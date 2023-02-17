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
    public class TransferRepository
    {
        private EntityDataContext _context;
        public TransferRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }
        public List<TransferViewModel> Search(TransferSearchViewModel searchViewModel)
        {
            searchViewModel.SearchFromStockId = searchViewModel.SearchFromStockId.Where(x => x != null).ToList();
            searchViewModel.SearchToStockId = searchViewModel.SearchToStockId.Where(x => x != null).ToList();
            searchViewModel.SearchProductId = searchViewModel.SearchProductId.Where(x => x != null).ToList();
            var query1 = (from s in _context.TransferModel
                          where (searchViewModel.isDelete == null ||
                              (
                                (searchViewModel.isDelete == true && s.isDeleted == true) ||
                                (searchViewModel.isDelete == false && (s.isDeleted == false || s.isDeleted == null))
                              )
                          )
                          //Công ty
                          && (searchViewModel.SearchCompanyId == null || s.CompanyId == searchViewModel.SearchCompanyId)
                          //Chi nhánh
                          && (searchViewModel.SearchStoreId == null || s.StoreId == searchViewModel.SearchStoreId)
                          //Ngày chứng từ
                          && (searchViewModel.SearchFromDate == null || searchViewModel.SearchFromDate <= s.DocumentDate)
                          //Ngày chưng từ
                          && (searchViewModel.SearchToDate == null || s.DocumentDate <= searchViewModel.SearchToDate)
                          //Mã
                          && (searchViewModel.SearchTransferCode == null || s.TransferCode.ToString().Contains(searchViewModel.SearchTransferCode))
                          //Nhân viên
                          && (searchViewModel.SearchSalesEmployeeCode == null || s.SalesEmployeeCode.ToString().Contains(searchViewModel.SearchSalesEmployeeCode))
                          select s);

            if (searchViewModel.SearchProductId != null || searchViewModel.SearchFromStockId != null || searchViewModel.SearchToStockId != null)
            {
                var query2 = (from detail in _context.TransferDetailModel
                              where
                              //Kho
                              (searchViewModel.SearchFromStockId.Count == 0 || searchViewModel.SearchFromStockId.Contains(detail.FromStockId))
                              && (searchViewModel.SearchToStockId.Count == 0 || searchViewModel.SearchToStockId.Contains(detail.ToStockId))
                              //Sản phẩm
                              && (searchViewModel.SearchProductId.Count == 0 || searchViewModel.SearchProductId.Contains(detail.ProductId))
                              group detail by detail.TransferId into g
                              select new { TransferId = g.Key });
                query1 = from master in query1
                         join detail in query2 on master.TransferId equals detail.TransferId
                         select master;
            }
            var res = query1.Select(p => new TransferViewModel
            {
                TransferId = p.TransferId,
                TransferCode = p.TransferCode,
                DocumentDate = p.DocumentDate,
                CompanyName = p.CompanyModel.CompanyName,
                StoreName = p.StoreModel.StoreName,
                EmployeeName = p.SalesEmployeeModel.SalesEmployeeName,
                Note = p.Note,
                CreateTime = p.CreateTime,
                isDeleted = p.isDeleted,
                StockTransferRequestId = p.StockTransferRequestId
            }).OrderByDescending(p => p.TransferCode).ToList();
            var temp = res.ToList().Take(50);
            if (res != null && res.Count > 0)
            {
                foreach (var item in temp)
                {
                    var ToStockId = _context.TransferDetailModel.Where(p => p.TransferId == item.TransferId)
                                            .Select(p => p.ToStockId).FirstOrDefault();
                    if (ToStockId != null)
                    {
                        var stockName = _context.StockModel.Where(p => p.StockId == ToStockId).FirstOrDefault();
                        if (stockName != null)
                        {
                            item.ToStockCode = stockName.StockName;
                        }
                    }
                    if (item.StockTransferRequestId != null)
                    {
                        var StockTransferRequestCode = _context.StockTransferRequestModel.Where(x => x.Id == item.StockTransferRequestId).FirstOrDefault();
                        if (StockTransferRequestCode != null)
                        {
                            item.StockTransferRequestCode = StockTransferRequestCode.StockTransferRequestCode;
                        }
                    }
                }
            }

            return temp.ToList();
        }
        public TransferModel Create(TransferViewModel viewModel)
        {
            var transferNew = new TransferModel();
            transferNew.MapTransfer(viewModel);
            transferNew.CreateBy = viewModel.CreateBy;
            transferNew.CreateTime = viewModel.CreateTime;
            _context.Entry(transferNew).State = EntityState.Added;
            return transferNew;
        }

        public TransferViewModel GetBy(Guid TransferId)
        {
            var result = (from p in _context.TransferModel
                          where p.TransferId == TransferId
                          select new TransferViewModel
                          {
                              TransferId = p.TransferId,
                              TransferCode = p.TransferCode,
                              DocumentDate = p.DocumentDate,
                              CompanyId = p.CompanyId,
                              CompanyName = p.CompanyModel.CompanyName,
                              StoreId = p.StoreId,
                              StoreName = p.StoreModel.StoreName,
                              SalesEmployeeCode = p.SalesEmployeeCode,
                              EmployeeName = p.SalesEmployeeModel.SalesEmployeeName,
                              Note = p.Note,
                              //Thông tin người gửi
                              SenderName = p.SenderName,
                              SenderPhone = p.SenderPhone,
                              SenderAddress = p.SenderAddress,
                              //Thông tin người nhận
                              RecipientName = p.RecipientName,
                              RecipientPhone = p.RecipientPhone,
                              RecipientCompany = p.RecipientCompany,
                              RecipientAddress = p.RecipientAddress,

                              CreateTime = p.CreateTime,
                              CreateBy = p.CreateBy,
                              isDeleted = p.isDeleted,
                              DeletedReason = p.DeletedReason
                          }).FirstOrDefault();
            return result;
        }
    }
}
