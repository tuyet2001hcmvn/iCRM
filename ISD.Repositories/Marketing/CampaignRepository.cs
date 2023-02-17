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
    public class CampaignRepository
    {
        private EntityDataContext _context;
        /// <summary>
        /// Khởi tạo ProductRepository truyển vào DataContext
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public CampaignRepository(EntityDataContext db)
        {
            _context = db;
        }
        /// <summary>
        /// Lấy thông tin Campaign
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CampaignModel GetBy(Guid? Id)
        {
            var data = _context.CampaignModel.Find(Id);
            return data;
        }

        public CampaignModel GetBy(int? Code)
        {
            var data = _context.CampaignModel.Where(x=>x.CampaignCode == Code).FirstOrDefault();
            return data;
        }

        public IEnumerable<CampaignModel> GetAll()
        {
            var data = _context.CampaignModel;
            return data;
        }
        public List<MemberOfTargetGroupInCampaignViewModel> MemberOfTargetGroupInCampaign(SendMailSearchViewModel viewModel)
        {
            var result = (from s in _context.SendMailCalendarModel
                          join c in _context.CampaignModel on s.CampaignId equals c.Id
                          where (viewModel.IsSend == null || (viewModel.IsSend == false && (s.IsSend == null || s.IsSend == false)) || s.IsSend == viewModel.IsSend)
                          && (viewModel.IsOpened == null || (viewModel.IsOpened == false && (s.IsOpened == null || s.IsOpened == false)) ||s.IsOpened == viewModel.IsOpened)
                          && (viewModel.IsBounce == null ||  s.IsBounce == viewModel.IsBounce)
                          && (viewModel.isConfirm == null || s.isConfirm == viewModel.isConfirm)
                          && (viewModel.isCheckin == null || (viewModel.isCheckin == false && (s.isCheckin == null || s.isCheckin == false) && s.isConfirm == true) || s.isCheckin == viewModel.isCheckin)
                          && (viewModel.CampaignId == null || s.CampaignId == viewModel.CampaignId)
                          && (viewModel.Type == null || c.Type == viewModel.Type)
                          orderby c.CampaignCode, s.FullName, s.IsSend, s.SendTime
                          select new MemberOfTargetGroupInCampaignViewModel
                          {
                              SendMailId = s.Id,
                              CampaignId = s.CampaignId,
                              CampainName = c.CampaignName,
                              Type = c.Type,
                              ProfileId = s.ToProfileId,
                              ProfileName = s.FullName,
                              Email = s.ToEmail,
                              //Email đã gửi
                              IsSend = s.IsSend,
                              SendTime = s.SendTime,
                              //Email đã mở
                              IsOpened = s.IsOpened,
                              LastOpenedTime = s.LastOpenedTime,
                              //Đồng ý tham dự
                              isConfirm = s.isConfirm,
                              //Đã checkin
                              isCheckin = s.isCheckin,
                              //Email lỗi
                              IsBounce = s.IsBounce,
                              //Thời gian đồng ý
                              ConfirmTime = s.ConfirmTime,
                              //Thời gian tham dự
                              CheckinTime = s.CheckinTime,
                              //Số nguười tham dự
                              NumberOfParticipant = s.NumberOfParticipant
                          }).OrderBy(x=>x.ProfileName).ToList();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    var totalInternalMember = (from c in _context.CampaignModel
                                               join m in _context.MemberOfTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                               where c.Id == item.CampaignId && m.ProfileId == item.ProfileId
                                               select m).FirstOrDefault();
                    if (totalInternalMember != null)
                    {
                        var profile = (from p in _context.ProfileModel
                                       where p.ProfileId == totalInternalMember.ProfileId
                                       select p).FirstOrDefault();
                        if (profile != null)
                        {
                            item.ProfileCode = profile.ProfileCode;
                            item.Phone = profile.Phone;
                        }
                    }
                    else
                    {
                        var totalExternalMember = (from c in _context.CampaignModel
                                                   join m in _context.MemberOfExternalProfileTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                                   where c.Id == item.CampaignId && m.ExternalProfileTargetGroupId == item.ProfileId
                                                   select m).FirstOrDefault();

                        if (totalExternalMember != null)
                        {
                            item.Phone = totalExternalMember.Phone;
                        }
                    }
                }
            }
            return result;
        }


    }
}