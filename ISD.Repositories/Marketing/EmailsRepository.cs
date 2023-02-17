using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using ISD.ViewModels.Marketing;
using ISD.ViewModels.Sale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace ISD.Repositories
{
    public class EmailsRepository
    {
        private EntityDataContext _context;
        /// <summary>
        /// Khởi tạo ProductRepository truyển vào DataContext
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public EmailsRepository(EntityDataContext db)
        {
            _context = db;
        }
        /// <summary>
        /// Lấy thông tin CheckIn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SendMailCalendarModel GetBy(Guid Id)
        {
            var data = _context.SendMailCalendarModel.Find(Id);
            return data;
        }

        public CheckInViewModel CheckIn(Guid sendMailCalendarId)
        {
            SendMailCalendarModel email = GetBy(sendMailCalendarId);
            if (email != null)
            {
                #region CheckIn
                email.isCheckin = true;
                email.CheckinTime = DateTime.Now;
                //Số người tham gia => lưu mặc định là 1 nếu = null
                if (!email.NumberOfParticipant.HasValue)
                {
                    email.NumberOfParticipant = 1;
                }
                
                _context.Entry(email).State = EntityState.Modified;
                _context.SaveChanges();
                #endregion

                //Thông tin cần trả về:
                //Tiêu đề

                var data = (from e in _context.SendMailCalendarModel
                            join c in _context.CampaignModel on e.CampaignId equals c.Id
                            join n in _context.ContentModel on c.ContentId equals n.Id
                            where e.Id == sendMailCalendarId
                            select new CheckInViewModel
                            {
                                Id = sendMailCalendarId,
                                CampaignId = c.Id,
                                Title = n.ContentName,
                                ProfileId = e.ToProfileId,
                                Email = e.ToEmail,
                                NumberOfParticipant = e.NumberOfParticipant.ToString(),
                            }).FirstOrDefault();
                //Họ và tên:
                //Email:
                //SĐT:
                //Ghi chú: 

                //Tìm thông tin trong MemberOfExternalProfileTargetGroupModel

                var totalInternalMember = (from c in _context.CampaignModel
                                           join m in _context.MemberOfTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                           where c.Id == data.CampaignId && m.ProfileId == data.ProfileId
                                           select m).FirstOrDefault();
                if (totalInternalMember != null)
                {
                    var profile = (from p in _context.ProfileModel
                                   where p.ProfileId == totalInternalMember.ProfileId
                                   select p).FirstOrDefault();
                    if (profile != null)
                    {
                        data.FullName = profile.ProfileName;
                        data.Phone = profile.Phone;
                    }
                }
                else
                {
                    var totalExternalMember = (from c in _context.CampaignModel
                                               join m in _context.MemberOfExternalProfileTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                               where c.Id == data.CampaignId && m.ExternalProfileTargetGroupId == data.ProfileId
                                               select m).FirstOrDefault();

                    if (totalExternalMember != null)
                    {
                        data.FullName = totalExternalMember.FullName;
                        data.Phone = totalExternalMember.Phone;
                    }
                }

                return data;

            }
            else
            {
                return null;
            }

        }


        public void ConfirmEmail(Guid sendMailCalendarId, string Type)
        {
            SendMailCalendarModel email = _context.SendMailCalendarModel.Find(sendMailCalendarId);
            if (email != null)
            {
                if (email.IsOpened == null || email.IsOpened == false)
                {
                    email.IsOpened = true;
                    email.LastOpenedTime = DateTime.Now;
                }

                if (Type == "Confirm")
                {
                    email.isConfirm = true;

                }
                else
                {
                    email.isConfirm = false;

                }
                email.ConfirmTime = DateTime.Now;
                _context.Entry(email).State = EntityState.Modified;
                _context.SaveChanges();

            }
        }

        public void UpdateQuantity(Guid sendMailCalendarId, int Quantity)
        {
            SendMailCalendarModel email = GetBy(sendMailCalendarId);
            if (email != null)
            {
                #region CheckIn
                email.NumberOfParticipant = Quantity;
                _context.Entry(email).State = EntityState.Modified;
                _context.SaveChanges();
                #endregion
            }
        }
    }
}