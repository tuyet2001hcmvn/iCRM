using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
   public class StockRecevingDetailRepository
    {
        private EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public StockRecevingDetailRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Thêm mới một Stock Receving Detail
        /// </summary>
        /// <param name="viewModel">Stock Receving Detail View Model</param>
        /// <returns>Stock Receving Detail Model</returns>
        public StockReceivingDetailModel Create(StockReceivingDetailViewModel viewModel)
        {
            var stockReDetailNew = new StockReceivingDetailModel();
            stockReDetailNew.MapStockRecevingDetail(viewModel);

            _context.Entry(stockReDetailNew).State = EntityState.Added;
            return stockReDetailNew;
        }

        public List<StockReceivingDetailViewModel> GetByStockReceiveMaster(Guid stockRecevingId)
        {
            var result = (from p in _context.StockReceivingDetailModel
                          where p.StockReceivingId == stockRecevingId
                          orderby p.ProductModel.ProductName
                          select new StockReceivingDetailViewModel
                          {
                              StockReceivingDetailId = p.StockReceivingDetailId,
                              ProductCode = p.ProductModel.ProductCode,
                              ProductName = p.ProductModel.ProductName,
                              StockCode = p.StockModel.StockCode,
                              StockName = p.StockModel.StockName,
                              DateKey = p.DateKey,
                              Quantity = p.Quantity,
                              Price = p.Price,
                              DetailNote = p.Note
                          }).ToList();
            return result;
        }
    }
}
