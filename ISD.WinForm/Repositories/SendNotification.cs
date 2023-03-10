using ISD.WinForm.Data;
using ISD.WinForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using MailKit.Security;
using ISD.EntityModels;
using System.Data.Entity;
using ISD.Repositories;

namespace ISD.WinForm.Repositories
{
    public class SendNotification
    {

        /// <summary>
        /// Khởi tạo repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public SendNotification()
        {
        }

        public void Send()
        {
            var _context = new EntityDataContext();
            var now = DateTime.Now;

            // Here we check 8:00.000 to 8:00.999 AM. Because clock runs every 1000ms, it should run the schedule
            if ((now.TimeOfDay >= new TimeSpan(0, 8, 00, 0, 0) && now.TimeOfDay <= new TimeSpan(0, 8, 00, 00, 999)))
            //if ((now.TimeOfDay >= new TimeSpan(0, 14, 22, 0, 0) && now.TimeOfDay <= new TimeSpan(0, 14, 22, 60, 999)))
            {
                // 8 AM schedule
                // Tìm những công việc đến hạn là ngày hiện tại và chưa hoàn thành
                var taskLst = (from t in _context.TaskModel
                               join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                               where (t.EstimateEndDate != null 
                               //&& DbFunctions.TruncateTime(t.EstimateEndDate) == DbFunctions.TruncateTime(now)
                               )
                               //chưa hoàn thành
                               && 
                               ts.ProcessCode != "completed"
                               select t
                              ).ToList();

                if (taskLst != null && taskLst.Count > 0)
                {
                    foreach (var item in taskLst)
                    {
                        string notificationMessage = string.Format("{0} cần được hoàn thành vào {1:dd/MM/yyyy}", item.Summary, item.EstimateEndDate);
                        //Check thông báo đã được gửi chưa
                        var existNotification = _context.NotificationModel.Where(p => p.TaskId == item.TaskId && p.Title == item.Summary && p.Description == notificationMessage).FirstOrDefault();
                        if (existNotification == null)
                        {
                            //Phạm vi gửi thông báo: trước 1 ngày
                            //Lấy ra 1 ngày trước ngày đến hạn
                            List<DateTime> nottificationDate = new List<DateTime>();
                            nottificationDate.Add(item.EstimateEndDate.Value.AddDays(-1).Date);
                            nottificationDate.Add(item.EstimateEndDate.Value.Date);
                            if (nottificationDate.Contains(now.Date))
                            {
                                //Tìm NV được phân công và gửi thông báo đến họ
                                PushNotification(item.TaskId, notificationMessage, item);
                            }
                        }
                    }
                }
            }
        }

        private void PushNotification(Guid TaskId, string notificationMessage, TaskModel task)
        {
            var _context = new EntityDataContext();
            //get device list except current user
            string[] deviceLst = new string[] { };
            string taskCode = string.Empty;
            List<Guid> accountLst = new List<Guid>();
            var deviceIdList = new List<string>();
            var workFlow = _context.WorkFlowModel.Where(p => p.WorkFlowId == task.WorkFlowId).FirstOrDefault();
            if (workFlow != null)
            {
                taskCode = workFlow.WorkFlowCode + "." + task.TaskCode;
            }

            var accountDeviceLst = _context.Account_Device_Mapping.ToList();

            //assignee
            var assigneeList = _context.TaskAssignModel.Where(p => p.TaskId == task.TaskId).ToList();
            if (assigneeList != null && assigneeList.Count > 0)
            {
                //Nếu phân công cho nhóm thì gửi thông báo đến tất cả nhân viên thuộc nhóm đó
                if (task.IsAssignGroup == true)
                {
                    var rolesCodeLst = assigneeList.Select(p => p.RolesCode).ToList();
                    var accountList = (from a in _context.AccountModel
                                       from r in a.RolesModel
                                       where rolesCodeLst.Contains(r.RolesCode)
                                       select a.AccountId
                                      ).ToList();
                    if (accountList != null && accountList.Count > 0)
                    {
                        var deviceAccountLst = accountDeviceLst.Where(p => accountList.Contains(p.AccountId)).Select(p => p.DeviceId).Distinct().ToList();
                        if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                        {
                            deviceIdList.AddRange(deviceAccountLst);
                        }

                        accountLst.AddRange(accountList);
                    }
                }
                else
                {
                    foreach (var taskAssign in assigneeList)
                    {
                        var account = _context.AccountModel.Where(p => p.EmployeeCode == taskAssign.SalesEmployeeCode).FirstOrDefault();
                        if (account != null)
                        {
                            var deviceAccountLst = accountDeviceLst.Where(p => p.AccountId == account.AccountId).Select(p => p.DeviceId).ToList();
                            if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                            {
                                deviceIdList.AddRange(deviceAccountLst);
                                accountLst.Add(account.AccountId);
                            }
                        }
                    }
                }
            }

            if (deviceIdList != null && deviceIdList.Count > 0)
            {
                deviceLst = deviceIdList.Distinct().ToArray();
            }

            //push notification
            string summary = task.Summary;
            UnitOfWork _unitOfWork = new UnitOfWork(_context);
            _unitOfWork.TaskRepository.PushNotification(TaskId, notificationMessage, deviceLst, summary, accountLst.Distinct().ToList());
            _context.SaveChanges();
        }
    }
}
