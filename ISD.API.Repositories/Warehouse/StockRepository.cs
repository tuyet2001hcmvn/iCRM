using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IStockRepository
    {
        StockOnHand GetStockOnHandBy(Guid StockId, Guid ProductId);
    }
    public class StockRepository : GenericRepository<StockModel>, IStockRepository
    {
        public StockRepository(ICRMDbContext context) : base(context)
        {

        }

        public StockOnHand GetStockOnHandBy(Guid StockId, Guid ProductId)
        {
            var onHandList = context.Set<StockOnHand>().FromSqlRaw("EXEC [dbo].[Warehouse_Stock_OnHand] @StockId, @ProductId", new SqlParameter("@StockId", StockId), new SqlParameter("@ProductId", ProductId)).AsEnumerable();
            return onHandList.FirstOrDefault();
        }
    }
}
