using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface IStoreService
    {
        IEnumerable<StoreViewModel> GetStores();
    }
}
