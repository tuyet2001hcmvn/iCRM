using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public class StoreRepository : GenericRepository<StoreModel>, IStoreRepository
    {
        public StoreRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<StoreModel> GetStores()
        {
            return context.StoreModels.Where(s => s.Actived == true).OrderBy(s=>s.SaleOrgCode);
        }
    }
}
