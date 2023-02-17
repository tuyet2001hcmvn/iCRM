using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public class StoreService : IStoreService
    {
        private readonly UnitOfWork _unitOfWork;
        public StoreService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<StoreViewModel> GetStores()
        {
            return _unitOfWork.StoreRepository.GetStores().Select(s => new StoreViewModel {
                StoreId = s.StoreId,
                StoreName = s.SaleOrgCode+ " | " + s.StoreName
            }).ToList();
        }
    }
}
