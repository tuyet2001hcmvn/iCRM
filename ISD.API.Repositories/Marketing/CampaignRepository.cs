using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class CampaignRepository : GenericRepository<CampaignModel>, ICampaignRepository
    {
        public CampaignRepository(ICRMDbContext context) : base(context)
        {        
        }
        public void CreateSendMailCalendarForNewCampaign(Guid id)
        {
            SqlParameter campaignId = new SqlParameter("@CampaignId", id);        
            context.Database.ExecuteSqlRaw("exec Marketing.CreateSendMailCalendar @CampaignId", campaignId);
        }

        public CampaignReportViewModel GetReportById(Guid id)
        {
            var campaign = (from c in context.CampaignModels
                           where c.Id == id
                           select c).FirstOrDefault();
            CampaignReportViewModel report = new CampaignReportViewModel();

            var totalInternalMember = from c in context.CampaignModels
                                 join member in context.MemberOfTargetGroupModels on c.TargetGroupId equals member.TargetGroupId
                                 where c.Id == id
                                 select member;
            var totalExternalMember = from c in context.CampaignModels
                                      join member in context.MemberOfExternalProfileTargetGroupModels on c.TargetGroupId equals member.TargetGroupId
                                      where c.Id == id
                                      select member;
            var totalEmail = from email in context.SendMailCalendarModels
                             where email.CampaignId == id
                             select email;
            var totalMailSend = from c in context.CampaignModels
                                join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                                where c.Id == id && sendMail.IsSend == true
                                select sendMail;
            var totalMailBounce = from c in context.CampaignModels
                                join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                                  where c.Id == id && (sendMail.IsBounce == true || sendMail.IsError == true)
                                  select sendMail;
            var totalMailOpened = from c in context.CampaignModels
                                  join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                                  where c.Id == id && sendMail.IsSend == true && sendMail.IsOpened == true
                                  select sendMail;
            if (campaign.Type == "Marketing")
            {
                report.TotalMember = totalEmail.Count();
                report.TotalMailSend = totalMailSend.Count();
                report.TotalMailBounce = totalMailBounce.Count();
                report.TotalMailOpened = totalMailOpened.Count();
            }
            if (campaign.Type == "Event")
            {
                report.TotalMember = totalEmail.Count();
                report.TotalMailSend = totalMailSend.Count();
                // Đồng ý tham gia event
                var totalConfirm = from c in context.CampaignModels
                                      join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                                      where c.Id == id &&  sendMail.IsConfirm == true
                                      select sendMail;
                var totalReject = from c in context.CampaignModels
                                      join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                                      where c.Id == id &&  sendMail.IsConfirm == false
                                  select sendMail;
                var Checkin = from c in context.CampaignModels
                              join sendMail in context.SendMailCalendarModels on c.Id equals sendMail.CampaignId
                              where c.Id == id && sendMail.IsCheckin == true
                              select sendMail;
                var PeopleCheckin = context.SendMailCalendarModels.Where(x => x.CampaignId == id).Sum(x => x.NumberOfParticipant);
                //Mail được mở
                report.TotalMailBounce = totalMailBounce.Count();
                report.TotalMailOpened = totalMailOpened.Count();
                report.TotalConfirm = totalConfirm.Count();
                report.TotalReject = totalReject.Count();
                report.TotalCheckin = Checkin.Count();
                report.PeopleCheckin = PeopleCheckin??0;
            }
            return report;
        }

        public IQueryable<CampaignModel> Search(int? campaignCode, string campaignName, string status, string type)
        {
            var campaigns = from t in context.CampaignModels.Include(t => t.TargetGroup).Include(t => t.Content).Include(t=>t.StatusNavigation)
                            where (campaignCode == null || t.CampaignCode == campaignCode) &&
                            (campaignName == null || campaignName == "" || t.CampaignName.Contains(campaignName)) &&
                             (status == null /*|| t.Status==(status)*/)
                              && t.Type == type
                            orderby (t.CampaignCode) descending
                           select t;         
            return campaigns;
        }

        public void UpdateSendMailCalendarByCampaign(Guid id)
        {
            SqlParameter campaignId = new SqlParameter("@CampaignId", id);
            context.Database.ExecuteSqlRaw("exec Marketing.UpdateSendMailCalendar @CampaignId", campaignId);
        }
    }
}
