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
    public class TransferDetailRepository
    {
        private EntityDataContext _context;
        public TransferDetailRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public TransferDetailModel Create(TransferDetailViewModel viewModel)
        {
            var transferDetailNew = new TransferDetailModel();
            transferDetailNew.MapTranferDetail(viewModel);
            _context.Entry(transferDetailNew).State = EntityState.Added;
            return transferDetailNew;
        }

        /// <summary>
        /// Get by TransferId
        /// </summary>
        /// <param name="TransferId"></param>
        /// <returns>List transfer detail</returns>
        public List<TransferDetailViewModel> GetBy(Guid TransferId)
        {
            var result = (from p in _context.TransferDetailModel
                          join fs in _context.StockModel on p.FromStockId equals fs.StockId
                          join ts in _context.StockModel on p.ToStockId equals ts.StockId
                          where p.TransferId == TransferId
                          orderby p.ProductModel.ProductName
                          select new TransferDetailViewModel
                          {
                              TransferDetailId = p.TransferDetailId,
                              TransferId = p.TransferId,
                              ProductId = p.ProductId,
                              ProductCode = p.ProductModel.ProductCode,
                              ProductName = p.ProductModel.ProductName,
                              FromStockId = p.FromStockId,
                              FromStockCode = fs.StockCode,
                              FromStockName = fs.StockCode + " | " + fs.StockName,
                              ToStockId = p.ToStockId,
                              ToStockCode = ts.StockCode,
                              ToStockName = ts.StockCode + " | " + ts.StockName,
                              DateKey = p.DateKey,
                              Quantity = p.Quantity,
                              Price = p.Price,
                              UnitPrice = p.UnitPrice,
                              DetailNote = p.Note
                          }).ToList();
            return result;
        }
    }
}
