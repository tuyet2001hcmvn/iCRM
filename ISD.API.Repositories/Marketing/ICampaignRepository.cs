using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public interface ICampaignRepository
    {
        void CreateSendMailCalendarForNewCampaign(Guid id);
        IQueryable<CampaignModel> Search(int? campaignCode, string campaignName, string status, string type);
        void UpdateSendMailCalendarByCampaign(Guid id);

        CampaignReportViewModel GetReportById(Guid id);
    }
}
