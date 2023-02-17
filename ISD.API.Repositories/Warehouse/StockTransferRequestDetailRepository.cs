using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IStockTransferRequestDetailRepository
    {
        IQueryable<StockTransferRequestDetailModel> GetTransferRequestDetailByTransferRequestId(Guid id);
        void DeleteTransferRequestDetailByTransferRequestId(Guid id);
    }
    public class StockTransferRequestDetailRepository : GenericRepository<StockTransferRequestDetailModel>, IStockTransferRequestDetailRepository
    {
        public StockTransferRequestDetailRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<StockTransferRequestDetailModel> GetTransferRequestDetailByTransferRequestId(Guid id)
        {
            return context.StockTransferRequestDetailModels.Include(s => s.Product).Where(s => s.StockTransferRequestId == id).OrderBy(x => x.Product.ProductCode);
        }
        public void DeleteTransferRequestDetailByTransferRequestId(Guid id)
        {
            var StockTransferRequestDetail = context.StockTransferRequestDetailModels.Where(s => s.StockTransferRequestId == id);
            if (StockTransferRequestDetail != null && StockTransferRequestDetail.Count() > 0)
            {
                foreach (var item in StockTransferRequestDetail)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
            }
        }
    }
}
