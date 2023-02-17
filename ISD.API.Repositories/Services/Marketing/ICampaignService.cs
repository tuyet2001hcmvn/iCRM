using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface ICampaignService
    {
        CampaignViewViewModel Create(CampaignCreateViewModel obj);
        CampaignViewViewModel GetById(Guid id);
        List<CampaignStatusViewModel> GetCampaignStatus();
        void Update(Guid id, CampaignEditViewModel obj);
        List<CampaignViewViewModel> Search(int? campaignCode, string campaignName, string status, int pageIndex, int pageSize, string type);
        int GetTotalRowSearch(int? campaignCode, string campaignName, string status, string type);

        CampaignReportViewModel GetReportForCampiagn(Guid id);

        void EmailIsOpened(Guid id);
    }
}

