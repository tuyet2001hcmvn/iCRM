using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Warehouse
{
    public class InventoryRepository
    {
        private EntityDataContext _context;
        public InventoryRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<InventoryViewModel> Search(Guid? companyId, Guid? StoreId, Guid? ProductId)
        {
            var result = _context.Database.SqlQuery<InventoryViewModel>("EXEC [dbo].[Warehouse_Search_Inventory] @CompanyId, @StoreId,@ProductId ",
                new SqlParameter("@CompanyId", companyId ?? (object)DBNull.Value),
                new SqlParameter("@ProductId", ProductId ?? (object)DBNull.Value),
                new SqlParameter("@StoreId", StoreId ?? (object)DBNull.Value)).ToList();
            return result;
        }

    }
}
