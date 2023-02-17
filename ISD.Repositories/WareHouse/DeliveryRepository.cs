using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ISD.Repositories
{
    public class DeliveryRepository
    {
        private EntityDataContext _context;
        public DeliveryRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<DeliveryViewModel> Search(DeliverySearchViewModel searchViewModel)
        {
            var query1 = (from s in _context.DeliveryModel

                          where (searchViewModel.isDeleted == null || 
                              (
                                (searchViewModel.isDeleted == true && s.isDeleted == true) || 
                                (searchViewModel.isDeleted == false && (s.isDeleted == false || s.isDeleted == null))
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
                          && (searchViewModel.SearchDeliveryCode == null || s.DeliveryCode.ToString().Contains(searchViewModel.SearchDeliveryCode.ToString()))
                          //Nhân viên
                          && (searchViewModel.SearchSalesEmployeeCode == null || s.SalesEmployeeCode.Contains(searchViewModel.SearchSalesEmployeeCode))
                          
                          select s);

            //handle xuất kho cũ: nếu trong phiếu xuất kho chỉ có những mã sp đã không còn sử dụng thì không hiển thị phiếu đó luôn
            var deliveryIdList = query1.Select(p => p.DeliveryId).ToList();
            var detailDelivery = (from d in _context.DeliveryDetailModel
                                  join p in _context.ProductModel on d.ProductId equals p.ProductId
                                  where deliveryIdList.Contains(d.DeliveryId.Value) && p.Actived == true
                                  select d.DeliveryId
                                  ).Distinct().ToList();
            if (detailDelivery != null && deliveryIdList.Count > 0)
            {
                query1 = query1.Where(p => detailDelivery.Contains(p.DeliveryId));
            }


            return query1.Select(s => new DeliveryViewModel
            {
                DeliveryId = s.DeliveryId,
                DeliveryCode = s.DeliveryCode,
                DocumentDate = s.DocumentDate,
                CompanyName = s.CompanyModel.CompanyName,
                StoreName = s.StoreModel.StoreName,
                ProfileId = s.ProfileId,
                ProfileName = s.ProfileModel.ProfileName,
                SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
                Note = s.Note,
                CreateTime = s.CreateTime,
                isDeleted = s.isDeleted

            }).OrderByDescending(p => p.DeliveryCode).ToList();
        }

        public List<DeliveryViewModel> SearchQuery(DeliverySearchViewModel searchViewModel, out int filteredResultsCount)
        {
            string sqlQuery = "EXEC [Warehouse].[usp_SearchDelivery_Dynamic] @SearchCompanyId, @SearchStoreId, @SearchProfileId, @SearchFromDate, @SearchToDate, @SearchDeliveryCode, @SearchSalesEmployeeCode, @isDeleted, @SearchCategoryId, @PageSize, @PageNumber, @FilteredResultsCount OUT";

            var FilteredResultsCountOutParam = new SqlParameter();
            FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchCompanyId",
                    Value = searchViewModel.SearchCompanyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchStoreId",
                    Value = searchViewModel.SearchStoreId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchProfileId",
                    Value = searchViewModel.SearchProfileId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchFromDate",
                    Value = searchViewModel.SearchFromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchToDate",
                    Value = searchViewModel.SearchToDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchDeliveryCode",
                    Value = searchViewModel.SearchDeliveryCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchSalesEmployeeCode",
                    Value = searchViewModel.SearchSalesEmployeeCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "isDeleted",
                    Value = searchViewModel.isDeleted ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchCategoryId",
                    Value = searchViewModel.SearchCategoryId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageSize",
                    Value = searchViewModel.PageSize,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageNumber",
                    Value = searchViewModel.PageNumber,
                },
                FilteredResultsCountOutParam
            };
            #endregion

            var result = _context.Database.SqlQuery<DeliveryViewModel>(sqlQuery, parameters.ToArray()).ToList();
            var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            if (filteredResultsCountValue != null)
            {
                filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            }
            else
            {
                filteredResultsCount = 0;
            }

            //return query1.Select(s => new DeliveryViewModel
            //{
            //    DeliveryId = s.DeliveryId,
            //    DeliveryCode = s.DeliveryCode,
            //    DocumentDate = s.DocumentDate,
            //    CompanyName = s.CompanyModel.CompanyName,
            //    StoreName = s.StoreModel.StoreName,
            //    ProfileId = s.ProfileId,
            //    ProfileName = s.ProfileModel.ProfileName,
            //    SalesEmployeeName = s.SalesEmployeeModel.SalesEmployeeName,
            //    Note = s.Note,
            //    CreateTime = s.CreateTime,
            //    isDeleted = s.isDeleted
            //}).OrderByDescending(p => p.DeliveryCode);

            return result;
        }

        public DeliveryModel Create(DeliveryViewModel viewModel)
        {
            var deliveryNew = new DeliveryModel();
            deliveryNew.MapDelivery(viewModel);
            deliveryNew.CreateBy = viewModel.CreateBy;
            deliveryNew.CreateTime = viewModel.CreateTime;
            deliveryNew.isDeleted = false;
            _context.Entry(deliveryNew).State = EntityState.Added;

            return deliveryNew;
        }

        public DeliveryDetailModel CreateDetail(DeliveryDetailViewModel viewModel)
        {
            var deliveryDetailNew = new DeliveryDetailModel();
            deliveryDetailNew.MapDeliveryDetail(viewModel);
            deliveryDetailNew.isDeleted = false;
            _context.Entry(deliveryDetailNew).State = EntityState.Added;

            return deliveryDetailNew;
        }

        public void Edit(DeliveryViewModel viewModel)
        {
            var delivery = _context.DeliveryModel.FirstOrDefault(p => p.DeliveryId == viewModel.DeliveryId);
            if (delivery != null)
            {
                delivery.ProfileId = viewModel.ProfileId;
                delivery.SenderName = viewModel.SenderName;
                delivery.SenderAddress = viewModel.SenderAddress;
                delivery.SenderPhone = viewModel.SenderPhone;

                delivery.RecipientName = viewModel.RecipientName;
                delivery.RecipientCompany = viewModel.RecipientCompany;
                delivery.RecipientAddress = viewModel.RecipientAddress;
                delivery.RecipientPhone = viewModel.RecipientPhone;
                delivery.ShippingTypeCode = viewModel.ShippingTypeCode;

                delivery.LastEditBy = viewModel.LastEditBy;
                delivery.LastEditTime = viewModel.LastEditTime;

                delivery.Note = viewModel.Note;
            }
            _context.Entry(delivery).State = EntityState.Modified;
        }

        public DeliveryViewModel GetBy(Guid deliveryId)
        {
            var result = (from p in _context.DeliveryModel
                          where p.DeliveryId == deliveryId
                          select new DeliveryViewModel
                          {
                              DeliveryId = p.DeliveryId,
                              DeliveryCode = p.DeliveryCode,
                              CompanyId = p.CompanyId,
                              CompanyName = p.CompanyModel.CompanyName,
                              StoreId = p.StoreId,
                              StoreName = p.StoreModel.StoreName,
                              SalesEmployeeCode = p.SalesEmployeeCode,
                              SalesEmployeeName = p.SalesEmployeeModel.SalesEmployeeName,
                              DocumentDate = p.DocumentDate,
                              ProfileId = p.ProfileId,
                              isDeleted = p.isDeleted,
                              DeletedReason = p.DeletedReason,
                              SenderName = p.SenderName,
                              SenderAddress = p.SenderAddress,
                              SenderPhone = p.SenderPhone,
                              RecipientName = p.RecipientName,
                              RecipientAddress = p.RecipientAddress,
                              RecipientPhone = p.RecipientPhone,
                              RecipientCompany = p.RecipientCompany,
                              ShippingTypeCode=p.ShippingTypeCode,
                              Note = p.Note,
                              TaskId = p.TaskId
                          }).FirstOrDefault();
            return result;
        }

        public List<DeliveryDetailViewModel> GetDetailByDeliveryId(Guid DeliveryId)
        {
            var result = (from p in _context.DeliveryDetailModel
                          join product in _context.ProductModel on p.ProductId equals product.ProductId
                          where p.DeliveryId == DeliveryId 
                          //&& product.Actived == true
                          orderby p.ProductModel.ProductName
                          select new DeliveryDetailViewModel
                          {
                              DeliveryDetailId = p.DeliveryDetailId,
                              DeliveryId = p.DeliveryId,
                              StockCode = p.StockModel.StockCode,
                              StockId = p.StockId,
                              StockName = p.StockModel.StockName,
                              ProductCode = product.ProductCode,
                              ProductId = p.ProductId,
                              ProductName = product.ProductName,
                              Quantity = p.Quantity,
                              Price = p.Price > 0 ? p.Price : product.Price,
                              DetailNote = p.Note
                          }).ToList();
            return result;
        }
    }
}
