using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISD.Extensions;

namespace ISD.Repositories
{
    public class StockReceivingMasterRepository
    {
        private EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public StockReceivingMasterRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Tìm kiếm Nhập kho
        /// </summary>
        /// <param name="searchViewModel">StockReceivingSearchViewModel</param>
        /// <returns>Danh sách nhập kho</returns>
        public List<StockReceivingViewModel> Search(StockReceivingSearchViewModel searchViewModel)
        {
            searchViewModel.SearchStockId = searchViewModel.SearchStockId.Where(x => x != null && x != Guid.Empty).ToList();
            searchViewModel.SearchProductId = searchViewModel.SearchProductId.Where(x => x != null && x != Guid.Empty).ToList();
            var query1 = (from s in _context.StockReceivingMasterModel
                              //   join sd in _context.StockRecevingDetailModel on s.StockReceivingId equals sd.StockReceivingId 
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
                          //Nhà cung cấp
                          && (searchViewModel.SearchProfileId == null || s.ProfileId == searchViewModel.SearchProfileId)
                          //Ngày chứng từ
                          && (searchViewModel.SearchFromDate == null || searchViewModel.SearchFromDate <= s.DocumentDate)
                          //Ngày chưng từ
                          && (searchViewModel.SearchToDate == null || s.DocumentDate <= searchViewModel.SearchToDate)
                          // && (searchViewModel.ProductId == null || sd.ProductId == searchViewModel.ProductId)
                          //Mã
                          && (searchViewModel.SearchStockReceivingCode == null || s.StockReceivingCode.ToString().Contains(searchViewModel.SearchStockReceivingCode))
                          //Nhân viên
                          && (searchViewModel.SearchSalesEmployeeCode == null || s.SalesEmployeeCode.ToString().Contains(searchViewModel.SearchSalesEmployeeCode))
                          //select new StockReceivingViewModel {
                          //    StockReceivingId = s.StockReceivingId,
                          //    StockReceivingCode = s.StockReceivingCode,
                          //    DocumentDate = s.DocumentDate,
                          //    CompanyName = s.CompanyModel.CompanyName,
                          //    ProfileName = s.ProfileModel.ProfileName,
                          //    SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
                          //    Note = s.Note,
                          //    CreateTime = s.CreateTime
                          //}
                          select s);
            if (searchViewModel.SearchStockId != null || searchViewModel.SearchProductId != null)
            {
                var query2 = (from detail in _context.StockReceivingDetailModel// on master.StockReceivingId equals detail.StockReceivingId
                              where
                              //Kho
                              (searchViewModel.SearchStockId.Count == 0 || searchViewModel.SearchStockId.Contains(detail.StockId.Value))
                              //Sản phẩm
                              && (searchViewModel.SearchProductId.Count == 0 || searchViewModel.SearchProductId.Contains(detail.ProductId.Value))
                              //&& (searchViewModel.isDelete == null || detail.isDeleted == searchViewModel.isDelete)
                              group detail by detail.StockReceivingId into g
                              select new { StockReceivingId = g.Key });
                query1 = from master in query1
                         join detail in query2 on master.StockReceivingId equals detail.StockReceivingId
                         select master;
            }


            return query1.Select(s => new StockReceivingViewModel
            {
                StockReceivingId = s.StockReceivingId,
                StockReceivingCode = s.StockReceivingCode,
                DocumentDate = s.DocumentDate,
                CompanyName = s.CompanyModel.CompanyName,
                StoreName = s.StoreModel.StoreName,
                ProfileId = s.ProfileId,
                ProfileName = s.ProfileModel.ProfileName,
                SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
                Note = s.Note,
                CreateTime = s.CreateTime,
                isDeleted = s.isDeleted

            }).OrderByDescending(p => p.StockReceivingCode).ToList();
        }
        public List<StockReceivingViewModel> SreachBeginInventory(StockReceivingSearchViewModel searchViewModel)
        {
            searchViewModel.SearchStockId = searchViewModel.SearchStockId.Where(x => x != null && x != Guid.Empty).ToList();
            searchViewModel.SearchProductId = searchViewModel.SearchProductId.Where(x => x != null && x != Guid.Empty).ToList();
            var query1 = (from s in _context.StockReceivingMasterModel
                              //   join sd in _context.StockRecevingDetailModel on s.StockReceivingId equals sd.StockReceivingId 
                          where s.isDeleted == searchViewModel.isDelete && s.isFirst == true
                          //Công ty
                          && (searchViewModel.SearchCompanyId == null || s.CompanyId == searchViewModel.SearchCompanyId)
                          //Chi nhánh
                          && (searchViewModel.SearchStoreId == null || s.StoreId == searchViewModel.SearchStoreId)
                          //Nhà cung cấp
                          && (searchViewModel.SearchProfileId == null || s.ProfileId == searchViewModel.SearchProfileId)
                          //Ngày chứng từ
                          && (searchViewModel.SearchFromDate == null || searchViewModel.SearchFromDate <= s.DocumentDate)
                          //Ngày chưng từ
                          && (searchViewModel.SearchToDate == null || s.DocumentDate <= searchViewModel.SearchToDate)
                          // && (searchViewModel.ProductId == null || sd.ProductId == searchViewModel.ProductId)
                          //Mã
                          && (searchViewModel.SearchStockReceivingCode == null || s.StockReceivingCode.ToString().Contains(searchViewModel.SearchStockReceivingCode))
                          //select new StockReceivingViewModel {
                          //    StockReceivingId = s.StockReceivingId,
                          //    StockReceivingCode = s.StockReceivingCode,
                          //    DocumentDate = s.DocumentDate,
                          //    CompanyName = s.CompanyModel.CompanyName,
                          //    ProfileName = s.ProfileModel.ProfileName,
                          //    SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
                          //    Note = s.Note,
                          //    CreateTime = s.CreateTime
                          //}
                          select s);
            if (searchViewModel.SearchStockId != null || searchViewModel.SearchProductId != null)
            {
                var query2 = (from detail in _context.StockReceivingDetailModel// on master.StockReceivingId equals detail.StockReceivingId
                              where
                              //Kho
                              (searchViewModel.SearchStockId.Count == 0 || searchViewModel.SearchStockId.Contains(detail.StockId.Value))
                              //Sản phẩm
                              && (searchViewModel.SearchProductId.Count == 0 || searchViewModel.SearchProductId.Contains(detail.ProductId.Value))
                              //&& (searchViewModel.isDelete == null || detail.isDeleted == searchViewModel.isDelete)
                              group detail by detail.StockReceivingId into g
                              select new { StockReceivingId = g.Key });

                query1 = from master in query1
                         join detail in query2 on master.StockReceivingId equals detail.StockReceivingId
                         select master;
            }


            return query1.Select(s => new StockReceivingViewModel
            {
                StockReceivingId = s.StockReceivingId,
                StockReceivingCode = s.StockReceivingCode,
                DocumentDate = s.DocumentDate,
                CompanyName = s.CompanyModel.CompanyName,
                StoreName = s.StoreModel.StoreName,
                ProfileName = s.ProfileModel.ProfileName,
                SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
                Note = s.Note,
                CreateTime = s.CreateTime,
                isDeleted = s.isDeleted

            }).OrderByDescending(p => p.StockReceivingCode).ToList();
        }
        /// <summary>
        /// Thêm mới một Stock Receiving Master
        /// </summary>
        /// <param name="viewModel">Stock Receiving View Model</param>
        /// <returns>Stock Receiving Master Model</returns>
        public StockReceivingMasterModel Create(StockReceivingViewModel viewModel)
        {
            var stockReceivingNew = new StockReceivingMasterModel();

            stockReceivingNew.MapStockReceivingMaster(viewModel);
            stockReceivingNew.CreateBy = viewModel.CreateBy;
            stockReceivingNew.CreateTime = viewModel.CreateTime;

            _context.Entry(stockReceivingNew).State = EntityState.Added;
            return stockReceivingNew;
        }
        public StockReceivingMasterModel Create(StockReceivingMasterViewModel viewModel)
        {
            var stockReceivingNew = new StockReceivingMasterModel();

            stockReceivingNew.MapStockReceivingMaster(viewModel);
            stockReceivingNew.CreateBy = viewModel.CreateBy;
            stockReceivingNew.CreateTime = viewModel.CreateTime;
            stockReceivingNew.isFirst = viewModel.isFirst;

            _context.Entry(stockReceivingNew).State = EntityState.Added;
            return stockReceivingNew;
        }

        public StockReceivingMasterViewModel GetById(Guid stockReceivingId)
        {
            var result = (from p in _context.StockReceivingMasterModel
                          where p.StockReceivingId == stockReceivingId
                          select new StockReceivingMasterViewModel
                          {
                              StockReceivingId = p.StockReceivingId,
                              StockReceivingCode = p.StockReceivingCode,
                              DocumentDate = p.DocumentDate,
                              CompanyId = p.CompanyId,
                              CompanyName = p.CompanyModel.CompanyName,
                              StoreId = p.StoreId,
                              StoreName = p.StoreModel.StoreName,
                              SalesEmployeeCode = p.SalesEmployeeCode,
                              SalesEmployeeName = p.SalesEmployeeModel.SalesEmployeeName,
                              ProfileId = p.ProfileId,
                              ProfileName = p.ProfileModel.ProfileName,
                              Note = p.Note,
                              isDeleted = p.isDeleted,
                              DeletedReason = p.DeletedReason
                          }).FirstOrDefault();
            return result;
        }
    }
}
