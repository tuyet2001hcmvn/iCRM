using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IStockTransferRequestRepository
    {
        IQueryable<StockTransferRequestModel> Search(Guid? companyId, Guid? storeId, int? stockTransferRequestCode, string salesEmployeeCode, Guid? fromStock, Guid? toStock, DateTime? fromDate, DateTime? toDate, bool? actived, out int totalRow);
    }
    public class StockTransferRequestRepository : GenericRepository<StockTransferRequestModel>, IStockTransferRequestRepository
    {
        public StockTransferRequestRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<StockTransferRequestModel> Search(Guid? companyId, Guid? storeId, int? stockTransferRequestCode, string salesEmployeeCode, Guid? fromStock, Guid? toStock, DateTime? fromDate, DateTime? toDate, bool? actived, out int totalRow)
        {
            totalRow = 0;
            var list = from request in context.StockTransferRequestModels.Include(s => s.Company).Include(s => s.Store).Include(s => s.FromStockNavigation).Include(s => s.ToStockNavigation).Include(s => s.CreateByNavigation).Include(s => s.StockTransferRequestDetailModels)
                       where (companyId == null || companyId == Guid.Empty || request.CompanyId == companyId)
                       && (storeId == null || storeId == Guid.Empty || request.StoreId == storeId)
                       && (stockTransferRequestCode == null || request.StockTransferRequestCode == stockTransferRequestCode)
                        && (salesEmployeeCode == null || salesEmployeeCode == "" || request.SalesEmployeeCode.Contains(salesEmployeeCode))
                        && (fromStock == null || fromStock == Guid.Empty || request.FromStock == fromStock)
                        && (toStock == null || toStock == Guid.Empty || request.ToStock == toStock)
                        && (fromDate == null || request.DocumentDate >= fromDate)
                        && (toDate == null || request.DocumentDate <= toDate)
                        && (actived == null || request.Actived == actived)
                       orderby (request.StockTransferRequestCode) descending
                       select request;
            totalRow = list.Count();
            return list;
        }
        public decimal? GetTransferredQuantityByPlanDate(Guid stockTransferRequestId, Guid? ProductId)
        {
            //Lấy ngày kế hoạch
            DateTime? fromPlanDate = null;
            DateTime? toPlanDate = null;
            var stockTransferRequest = context.StockTransferRequestModels.Where(x => x.Id == stockTransferRequestId).FirstOrDefault();
            if (stockTransferRequest != null)
            {
                fromPlanDate = stockTransferRequest.FromPlanDate;
                toPlanDate = stockTransferRequest.ToPlanDate;
            }
            //Lấy số lượng đã chuyển
            var data = (from a in context.TransferDetailModels
                        join p in context.TransferModels on a.TransferId equals p.TransferId
                        join d in context.DimDateModels on a.DateKey equals d.DateKey
                        where a.ProductId == ProductId && a.IsDeleted != true && p.IsDeleted != true
                        && fromPlanDate <= d.Date && d.Date <= toPlanDate
                        && a.FromStockId == stockTransferRequest.FromStock
                        && a.ToStockId == stockTransferRequest.ToStock
                        select a.Quantity).Sum();
                       
            return data;
        }


        //public StockTransferRequestModel GetStockTransferRequestId (Guid StockTransferRequestId)
        //{
        //    StockTransferRequestModel entity = context.StockTransferRequestModels.Find()
        //}
    }
}
