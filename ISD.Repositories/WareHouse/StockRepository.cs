using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class StockRepository
    {
        EntityDataContext _context;
        public StockRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<StockModel> GetAll()
        {
            var stockList = _context.StockModel.Where(p => p.Actived == true).OrderBy(p => p.StockCode).ToList();
            return stockList;
        }

        public List<StockViewModel> GetAllForDropdown()
        {
            var result = _context.StockModel.Where(p => p.Actived == true).Select(p => new StockViewModel {
                StockId = p.StockId,
                StockCode = p.StockCode,
                StockName = p.StockCode + " | " + p.StockName
            }).OrderBy(p => p.StockCode).ToList();
            return result;
        }

        public Guid GetStockIdByStockCode(string StockCode)
        {
            return _context.StockModel.FirstOrDefault(p => p.StockCode == StockCode).StockId;
        }

        public List<StockViewModel> GetStockByStore(Guid StoreId)
        {
            var listStock = (from s in _context.StockModel
                             join sm in _context.Stock_Store_Mapping on s.StockId equals sm.StockId
                             where sm.StoreId == StoreId
                             orderby s.StockCode
                             select new StockViewModel
                             {
                                 StockId = s.StockId,
                                 StockName = s.StockCode + " | " + s.StockName,
                                 StockCode = s.StockCode,
                             }).ToList();
            return listStock;
        }
        //Check tồn kho hiện tại theo: Chi nhánh (lấy kho mặc định)
        public List<StockOnHandViewModel> GetStockOnHandBySaleOrg(string SaleOrgCode)
        {
            var onHandList = new List<StockOnHandViewModel>();
            //Get stock from SaleOrgCode
            var store = _context.StoreModel.Where(p => p.SaleOrgCode == SaleOrgCode).FirstOrDefault();
            if (store != null)
            {
                var stock = _context.Stock_Store_Mapping.Where(p => p.StoreId == store.StoreId && p.isMain == true).FirstOrDefault();
                onHandList = _context.Database.SqlQuery<StockOnHandViewModel>("EXEC [dbo].[Warehouse_Stock_OnHand] @StockId",
                                                                                new SqlParameter("@StockId", stock.StockId)
                                                                              ).ToList();
            }
            return onHandList;
        }
        //Check tồn kho hiện tại theo: Chi nhánh (lấy kho mặc định) và chỉ lấy loại catalogue
        public List<StockOnHandViewModel> GetStockOnHandBySaleOrgCatalogue()
        {
            var onHandList = new List<StockOnHandViewModel>();
            //Get stock from SaleOrgCode
            onHandList = _context.Database.SqlQuery<StockOnHandViewModel>("EXEC [Warehouse].[Warehouse_Stock_OnHand_Catalogue]").ToList();
            return onHandList;
        }
        //Check tồn kho hiện tại theo: Chi nhánh (lấy kho mặc định) + sản phẩm
        public StockOnHandViewModel GetStockOnHandBy(string SaleOrgCode, Guid ProductId)
        {
            var onHandList = new StockOnHandViewModel();
            //Get stock from SaleOrgCode
            var store = _context.StoreModel.Where(p => p.SaleOrgCode == SaleOrgCode).FirstOrDefault();
            if (store != null)
            {
                var stock = _context.Stock_Store_Mapping.Where(p => p.StoreId == store.StoreId && p.isMain == true).FirstOrDefault();
                onHandList = _context.Database.SqlQuery<StockOnHandViewModel>("EXEC [dbo].[Warehouse_Stock_OnHand] @StockId, @ProductId",
                                                                                new SqlParameter("@StockId", stock.StockId),
                                                                                new SqlParameter("@ProductId", ProductId)
                                                                              ).FirstOrDefault();
            }
            return onHandList;
        }
        /// <summary>
        /// Check tồn kho hiện tại theo: kho + sản phẩm
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public StockOnHandViewModel GetStockOnHandBy(Guid StockId, Guid ProductId)
        {
            var onHandList = _context.Database.SqlQuery<StockOnHandViewModel>("EXEC [dbo].[Warehouse_Stock_OnHand] @StockId, @ProductId",
                                                                            new SqlParameter("@StockId", StockId),
                                                                            new SqlParameter("@ProductId", ProductId)
                                                                              ).FirstOrDefault();
            return onHandList;
        }

        /// <summary>
        /// Tìm kiếm kho bởi chi nhánh, không có mã trước tên kho
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns>danh sách kho</returns>
        public List<StockViewModel> GetStockByStore2(Guid StoreId)
        {
            var listStock = (from s in _context.StockModel
                             join sm in _context.Stock_Store_Mapping on s.StockId equals sm.StockId
                             where sm.StoreId == StoreId
                             orderby s.StockCode
                             select new StockViewModel
                             {
                                 StockId = s.StockId,
                                 StockName = s.StockName,
                                 StockCode = s.StockCode,
                             }).ToList();
            return listStock;
        }

        /// <summary>
        /// get stock cho autocomple
        /// </summary>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        public List<StockAutocompleViewModel> GetForAutocomple(string SearchText)
        {
            var result = (from p in _context.StockModel
                          where p.StockCode.Contains(SearchText) || p.StockName.Contains(SearchText)
                          orderby p.StockCode
                          select new StockAutocompleViewModel
                          {
                              StockId = p.StockId,
                              StockName = p.StockName,
                              StockCode = p.StockCode,
                              StockCodeText = p.StockCode + " | " + p.StockName
                          }).Take(5).ToList();
            return result;
        }

        /// <summary>
        /// Get To Stock (Đã loại trừ kho FromStock))
        /// </summary>
        /// <param name="SearchText">SearchText</param>
        /// <param name="FromStockId">FromStockId</param>
        /// <returns>Danh sách kho</returns>
        public List<StockAutocompleViewModel> GetToStockForAutocomple(string SearchText, Guid? FromStockId)
        {
            var result = (from p in _context.StockModel
                          where p.StockId != FromStockId && (p.StockCode.Contains(SearchText) || p.StockName.Contains(SearchText))
                          orderby p.StockCode
                          select new StockAutocompleViewModel
                          {
                              StockId = p.StockId,
                              StockName = p.StockName,
                              StockCode = p.StockCode,
                              StockCodeText = p.StockCode + " | " + p.StockName
                          }).Take(5).ToList();
            return result;
        }
    }
}
