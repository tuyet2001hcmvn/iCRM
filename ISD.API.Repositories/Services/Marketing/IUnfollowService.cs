using ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface IUnfollowService
    {
        List<UnfollowViewModel> Search(string email,string companyCode, int page, int pageSize);
        int GetTotalRow(string email, string companyCode);

        void Create(UnfollowCreateViewModel obj);
        void Delete(Guid id);
    }
}
