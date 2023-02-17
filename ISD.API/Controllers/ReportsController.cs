using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.ViewModels;
using ISD.ViewModels.API;
using ISD.ViewModels.Sale;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISD.API.Controllers
{
    public class ReportsController : BaseController
    {
        #region Register
        private AccountRepository _accountRepo;
        private ReportRepository _reportRepo;
        public ReportsController()
        {
            _accountRepo = new AccountRepository(_context);
            _reportRepo = new ReportRepository(_context);
        }
        #endregion

        // GET: Reports
        #region Helper get master data

        #region Get user list
        public ActionResult GetUser(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                        .Select(p => new ISDSelectStringItem()
                                        {
                                            id = p.SalesEmployeeCode,
                                            name = p.SalesEmployeeName,
                                        });
                return _APISuccess(saleEmployeeList);
            });
        }
        #endregion Get user list

        #region Get company
        public ActionResult GetCompany(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var companyList = _unitOfWork.CompanyRepository.GetAll()
                                        .Select(p => new ISDSelectGuidItem()
                                        {
                                            id = p.CompanyId,
                                            name = p.CompanyName,
                                        });
                return _APISuccess(companyList);
            });
        }
        #endregion Get company

        #region Get workflow category
        public ActionResult GetWorkFlowCategory(string ReportType, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var workFlowCategoryList = _context.WorkFlowCategoryModel.Where(p => string.IsNullOrEmpty(ReportType) || p.ReportType.Contains(ReportType))
                    .Select(p => new ISDSelectStringItem()
                    {
                        id = p.WorkFlowCategoryCode,
                        name = p.WorkFlowCategoryName,
                    })
                    .ToList();
                return _APISuccess(workFlowCategoryList);
            });
        }
        #endregion Get workflow category

        #region Get department
        public ActionResult GetDepartment(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var departmentList = _context.DepartmentModel.Where(p => p.Actived == true).OrderBy(p => p.DepartmentCode)
                    .Select(p => new ISDSelectGuidItem()
                    {
                        id = p.DepartmentId,
                        name = p.DepartmentName,
                    })
                    .ToList();
                return _APISuccess(departmentList);
            });
        }
        #endregion Get department

        #region Get category
        public ActionResult GetCategory(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Loại Catalogue: Loại CTL là "Nhóm vật tư" có mã Like "CTL_%"
                var categoryList = _context.CategoryModel.Where(p => p.CategoryCode.Contains("CTL_"))
                                                        .Select(p => new ISDSelectGuidItem()
                                                        {
                                                            id = p.CategoryId,
                                                            name = p.CategoryName,
                                                        }).ToList();
                return _APISuccess(categoryList);
            });
        }
        #endregion Get category

        #endregion Helper get master data

        #region Báo cáo danh sách khách hàng ghé thăm
        /// <summary>
        /// 1. Báo cáo Danh sách Khách hàng
        /// </summary>
        /// <param name="searchViewModel">
        /// 1. Chi nhánh
        /// 1. Ngày nhập (từ … đến …)
        /// 2. User
        /// 3. Nguồn KH
        /// 5. Nhóm KH
        /// </param>
        /// <returns>
        /// 1. STT
        /// 2. Chủ đề
        /// 3. Trạng thái
        /// 4. Tên khách hàng
        /// 5. Số điện thoại
        /// 6. Địa chỉ
        /// 7. Email
        /// 8. Nhóm khách hàng
        /// 9. NV tiếp khách
        /// 10. Thời gian bắt đầu
        /// 11. Thời gian kết thúc
        /// 12. Ghi chú
        /// 13. Nguồn khách hàng
        /// 14. Chi nhánh
        /// 15. Ngày ghé thăm
        /// 16. Thị hiếu khách hàng
        /// 17. Catalogue đã xuất
        /// </returns>

        public ActionResult ProfileAppointmentReport(AppointmentSearchViewModel searchViewModel, Guid? AccountId, string FromDate_String, string ToDate_String, string SaleEmployeeCode, string CurrentCompanyCode, string token, string key)
        {
            //return ExecuteAPIContainer(token, key, () =>
            //{
                searchViewModel.SalesEmployeeCode = SaleEmployeeCode;
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1);
                }
                if (!string.IsNullOrEmpty(CurrentCompanyCode))
                {
                    searchViewModel.CompanyCode = CurrentCompanyCode;
                }

                //Nếu tài khoản xem theo chi nhánh thì chỉ lấy các chi nhánh được phân quyền
                if (searchViewModel.isViewByStore == true)
                {
                    if (searchViewModel.StoreId == null || searchViewModel.StoreId.Count == 0)
                    {
                        var storeList = _unitOfWork.StoreRepository.GetAllStore(searchViewModel.isViewByStore.Value, AccountId);
                        if (storeList != null && storeList.Count > 0)
                        {
                            searchViewModel.StoreId = new List<Guid>();
                            searchViewModel.StoreId = storeList.Select(p => p.StoreId).ToList();
                        }
                    }
                }

                var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);

                #region Số thứ tự, tổng số kết quả
                var result = new List<AppointmentViewModel>();
                if (query != null)
                {
                    result = query.ToList();
                }
                if (result != null && result.Count() > 0)
                {
                    int i = 0;
                    foreach (var item in query)
                    {
                        i++;
                        item.STT = i;

                        //Thị hiếu KH
                        var customerTasteLst = _unitOfWork.CustomerTasteRepository.GetCustomerTastesBy(item.AppointmentId);
                        if (customerTasteLst != null && customerTasteLst.Count > 0)
                        {
                            item.customerTasteLst = string.Join<string>(", ", customerTasteLst.Select(p => p.ProductCode));
                        }

                        //Catalogue
                        var customerCatalogueLst = _unitOfWork.CatalogueRepository.GetCustomerCatalogueBy(item.ProfileId.Value, item.VisitDate.Value);
                        if (customerCatalogueLst != null && customerCatalogueLst.Count > 0)
                        {
                            item.customerCatalogueLst = string.Join<string>(", ", customerCatalogueLst.Select(p => p.customerCatalogueLst));
                        }
                    }
                }
                #endregion
                //Tổng số KH
                int total = 0;
                if (result != null && result.Count > 0)
                {
                    total = result.Count();
                }

                return _APISuccess(new { Result = result, Total = total });
            //});
        }

        public ActionResult ProfileListReport(ProfileReportSearchViewModel searchModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<ProfileReportResultViewModel> result = new List<ProfileReportResultViewModel>();

                result = _reportRepo.ProfileListReport(searchModel);
                //Tổng số KH
                int total = 0;
                if (result != null && result.Count > 0)
                {
                    total = result.Count();
                }

                return _APISuccess(new { Result = result, Total = total });
            });
        }
        #endregion Báo cáo danh sách khách hàng ghé thăm

        #region Báo cáo phân loại khách hàng
        /// <summary>
        /// Báo cáo phân loại khách hàng: Tổng số khách hàng là tiêu dùng/doanh nghiệp
        /// </summary>
        /// <param name="searchViewModel">
        /// 1. Chi nhánh
        /// 1. Ngày nhập (từ … đến …)
        /// 2. User
        /// 3. Nguồn KH
        /// 5. Nhóm KH
        /// </param>
        /// <returns>
        /// 1. STT
        /// 2. Phân loại KH
        /// 3. Số lượng
        /// </returns>

        public ActionResult CustomerTypeReport(AppointmentSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1);
                }

                var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);

                #region Số thứ tự, tổng số kết quả
                var result = new List<CustomerTypeReportViewModel>();

                var queryLst = query.ToList();

                //Business
                result.Add(new CustomerTypeReportViewModel()
                {
                    PhanLoaiKH = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType)
                                        .Where(p => p.CatalogCode == ConstCustomerType.Bussiness)
                                        .Select(p => p.CatalogText_vi)
                                        .FirstOrDefault(),
                    SoLuong = queryLst.Where(p => p.CustomerTypeCode == ConstCustomerType.Bussiness).Count(),
                    CustomerTypeCode = ConstCustomerType.Bussiness,
                });

                //Customer
                result.Add(new CustomerTypeReportViewModel()
                {
                    PhanLoaiKH = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType)
                                            .Where(p => p.CatalogCode == ConstCustomerType.Customer)
                                            .Select(p => p.CatalogText_vi)
                                            .FirstOrDefault(),
                    SoLuong = queryLst.Where(p => p.CustomerTypeCode == ConstCustomerType.Customer).Count(),
                    CustomerTypeCode = ConstCustomerType.Customer,
                });

                if (!string.IsNullOrEmpty(searchViewModel.CustomerTypeCode))
                {
                    result = result.Where(p => p.CustomerTypeCode == searchViewModel.CustomerTypeCode).ToList();
                }

                if (result != null && result.Count > 0)
                {
                    int i = 0;
                    foreach (var item in result)
                    {
                        i++;
                        item.STT = i;
                    }
                }

                //Tổng cộng
                result.Add(new CustomerTypeReportViewModel()
                {
                    PhanLoaiKH = "Tổng cộng",
                    SoLuong = result.Sum(p => p.SoLuong),
                });
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo phân loại khách hàng

        #region Báo cáo nhóm khách hàng
        /// <summary>
        /// Báo cáo theo Nhóm Khách hàng
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Nhóm KH
        /// 3. Số lượng
        /// </returns>
        public ActionResult CustomerGroupReport(ProfileSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string SaleEmployeeCode, string CurrentCompanyCode, string CurrentUserName, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.CreateFromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.CreateToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1);
                }

                searchViewModel.SalesEmployeeCode = SaleEmployeeCode;
                searchViewModel.Type = ConstProfileType.Account;
                searchViewModel.Actived = true;
                searchViewModel.CreateRequestAll = "-- Tất cả --";
                searchViewModel.CustomerAccountGroupAll = true;
                //var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);
                int filteredResultsCount;
                var currentAccountId = _context.AccountModel.Where(p => p.FullName == CurrentUserName).Select(p => p.AccountId).FirstOrDefault();
                var query = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, currentAccountId, CurrentCompanyCode, out filteredResultsCount);
                #region Số thứ tự, tổng số kết quả
                var result = new List<CustomerGroupReportViewModel>();

                var queryLst = query.ToList();

                if (queryLst != null && queryLst.Count > 0)
                {
                    var customerGroupLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerGroup);
                    if (customerGroupLst != null && customerGroupLst.Count > 0)
                    {
                        var customerGroupCodeLst = queryLst.Select(x => x.CustomerGroupName).ToList();
                        customerGroupLst = customerGroupLst.Where(p => customerGroupCodeLst.Contains(p.CatalogText_vi)).ToList();
                        foreach (var customerGroup in customerGroupLst)
                        {
                            result.Add(new CustomerGroupReportViewModel()
                            {
                                NhomKH = customerGroup.CatalogText_vi,
                                SoLuong = queryLst.Where(p => p.CustomerGroupName == customerGroup.CatalogText_vi).Count(),
                                CustomerGroupCode = customerGroup.CatalogCode,
                            });
                        }
                    }
                }
                if (searchViewModel.CustomerGroupCode != null && searchViewModel.CustomerGroupCode.Count() > 0 && !string.IsNullOrEmpty(searchViewModel.CustomerGroupCode.FirstOrDefault()))
                {
                        //result = result.Where(p => p.CustomerGroupCode == searchViewModel.CustomerGroupCode).ToList();
                        result = (from p in result
                                  join a in searchViewModel.CustomerGroupCode on p.CustomerGroupCode equals a
                                  select p).ToList();
                }

                if (result != null && result.Count > 0)
                {
                    int i = 0;
                    foreach (var item in result)
                    {
                        i++;
                        item.STT = i;
                    }
                }

                //Tổng cộng
                result.Add(new CustomerGroupReportViewModel()
                {
                    NhomKH = "Tổng cộng",
                    SoLuong = result.Sum(p => p.SoLuong),
                });
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo nhóm khách hàng

        #region Báo cáo xuất Catalogue theo KH
        /// <summary>
        /// Báo cáo xuất Catalogue theo KH
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Tên khách hàng
        /// 3. Mã Catalogue
        /// 4. Tên Catalogue
        /// 5. Số lượng
        /// </returns>
        public ActionResult CustomerCatalogueReport(AppointmentSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1);
                }

                var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);

                #region Số thứ tự, tổng số kết quả
                var result = new List<CustomerCatalogueReportViewModel>();

                var queryLst = query.ToList();
                if (queryLst != null && queryLst.Count > 0)
                {
                    int i = 0;
                    foreach (var item in queryLst)
                    {
                        //Catalogue
                        var customerCatalogueLst = _unitOfWork.CatalogueRepository.GetCustomerCatalogueBy(item.ProfileId.Value, item.VisitDate.Value);
                        if (customerCatalogueLst != null && customerCatalogueLst.Count > 0)
                        {
                            foreach (var customerCatalogue in customerCatalogueLst)
                            {
                                i++;
                                result.Add(new CustomerCatalogueReportViewModel()
                                {
                                    TenKhachHang = item.ProfileName,
                                    MaCatalogue = customerCatalogue.ProductCode,
                                    TenCTL = customerCatalogue.ProductName,
                                    SoLuong = (int)customerCatalogue.Quantity,
                                });
                            }
                        }
                    }
                }

                //Group by và sum thêm lần nữa sau khi lọc theo Appointment
                if (result != null && result.Count > 0)
                {
                    result = result.GroupBy(item => new { item.TenKhachHang, item.MaCatalogue, item.TenCTL })
                             .Select(group => new CustomerCatalogueReportViewModel()
                             {
                                 TenKhachHang = group.Key.TenKhachHang,
                                 MaCatalogue = group.Key.MaCatalogue,
                                 TenCTL = group.Key.TenCTL,
                                 SoLuong = group.Sum(p => p.SoLuong),
                             }).ToList();

                    int i = 0;
                    foreach (var item in result)
                    {
                        i++;
                        item.STT = i;
                    }
                }

                //Tổng cộng
                result.Add(new CustomerCatalogueReportViewModel()
                {
                    TenCTL = "Tổng cộng",
                    SoLuong = result.Sum(p => p.SoLuong),
                });
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo xuất Catalogue theo KH

        #region Báo cáo xuất Catalogue
        /// <summary>
        /// Báo cáo xuất Catalogue
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Mã Catalogue
        /// 3. Tên Catalogue
        /// 4. ĐVT
        /// 5. Số lượng
        /// </returns>
        public ActionResult CatalogueReport(CatalogueReportSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1);
                }

                var result = _unitOfWork.CatalogueRepository.GetCatalogueReport(searchViewModel);

                if (result != null && result.Count > 0)
                {
                    //Tổng cộng
                    result.Add(new CatalogueReportViewModel()
                    {
                        DVT = "Tổng cộng",
                        SoLuong = result.Sum(p => p.SoLuong),
                    });
                }

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo xuất Catalogue

        #region Báo cáo thị hiếu KH
        /// <summary>
        /// Báo cáo thị hiếu khách hàng
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Mã màu
        /// 3. Showroom/Chi nhánh
        /// 4. Số lượt liked
        /// </returns>
        public ActionResult CustomerTastesReport(CustomerTastesSearchViewModel customer, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(customer.FromDate_String))
                {
                    customer.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(customer.FromDate_String);
                }
                if (!string.IsNullOrEmpty(customer.ToDate_String))
                {
                    customer.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(customer.ToDate_String);
                    if (customer.ToDate.HasValue)
                    {
                        customer.ToDate = customer.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    }
                }

                var result = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(customer);
                //Tổng cộng
                result.Add(new CustomerTastesReportViewModel()
                {
                    TenSP = "Tổng cộng",
                    SoLuotLiked = result.Sum(p => p.SoLuotLiked),
                });

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo thị hiếu KH

        #region Báo cáo số lượng KH showroom
        /// <summary>
        /// Báo cáo số lượng khách hàng showroom
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Chi nhánh/NPP/SRNQ
        /// 3. Nguồn khách hàng(ShowRoom/QRNQ)
        /// 4. Số lượng khách
        /// </returns>
        public ActionResult CustomerQuantityReport(AppointmentSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1); ;
                }

                var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);

                #region Số thứ tự, tổng số kết quả
                var result = new List<CustomerQuantityReportViewModel>();

                var queryLst = query.ToList();
                if (queryLst != null && queryLst.Count > 0)
                {
                    result = queryLst.Select(p => new CustomerQuantityReportViewModel()
                    {
                        SaleOrg = p.StoreName,
                        CustomerSource = p.ShowroomName,
                    }).ToList();

                    if (result != null && result.Count > 0)
                    {
                        result = result.GroupBy(p => new { p.SaleOrg, p.CustomerSource }).Select(p => new CustomerQuantityReportViewModel()
                        {
                            SaleOrg = p.Key.SaleOrg,
                            CustomerSource = p.Key.CustomerSource,
                            CustomerQuantity = p.Count(),
                        }).ToList();
                    }
                }
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo số lượng KH showroom

        #region Báo cáo số lượng khách hàng nhân viên tư vấn
        /// <summary>
        /// Báo cáo số lượng khách hàng nhân viên tư vấn
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Nhân viên
        /// 3. Chi nhánh/NPP/SRNQ
        /// 4. Nguồn khách hàng(ShowRoom/QRNQ)
        /// 5. Số lượng khách
        /// </returns>
        public ActionResult CustomerQuantityWithUserReport(AppointmentSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string SaleEmployeeCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(SaleEmployeeCode))
                {
                    searchViewModel.SalesEmployeeCode = SaleEmployeeCode;
                }
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1); ;
                }

                var query = _unitOfWork.AppointmentRepository.SearchQuery(searchViewModel);

                #region Số thứ tự, tổng số kết quả
                var result = new List<CustomerQuantityReportViewModel>();

                var queryLst = query.ToList();
                if (queryLst != null && queryLst.Count > 0)
                {
                    result = queryLst.Select(p => new CustomerQuantityReportViewModel()
                    {
                        Employee = p.SaleEmployeeName,
                        SaleOrg = p.StoreName,
                        CustomerSource = p.ShowroomName,
                    }).ToList();

                    if (result != null && result.Count > 0)
                    {
                        result = result.GroupBy(p => new { p.Employee, p.SaleOrg, p.CustomerSource }).Select(p => new CustomerQuantityReportViewModel()
                        {
                            Employee = p.Key.Employee,
                            SaleOrg = p.Key.SaleOrg,
                            CustomerSource = p.Key.CustomerSource,
                            CustomerQuantity = p.Count(),
                        }).ToList();
                    }
                }
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo số lượng khách hàng nhân viên tư vấn

        #region Báo cáo số lượng hoạt động CSKH theo nhân viên
        /// <summary>
        /// Báo cáo số lượng hoạt động CSKH theo nhân viên
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>
        /// 1. STT
        /// 2. Nhân viên kinh doanh
        /// 3. Phòng ban
        /// 4. Số khách ghé thăm
        /// 5. Số thăm hỏi khách hàng
        /// 6. Số bảo hành
        /// 7. Số xử lý khiếu nại
        /// 8. Số nhiệm vụ
        /// 9. Tổng các hoạt động
        /// </returns>
        public ActionResult CustomerCareActivitiesReport(CustomerCareSearchViewModel searchViewModel, string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    searchViewModel.StartFromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    searchViewModel.StartToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1); ;
                }

                //Nếu không chọn nhân viên nào thì add tất cả nhân viên
                if (searchViewModel.SalesEmployeeCode == null || searchViewModel.SalesEmployeeCode.Count == 0)
                {
                    var salesEmployeeCodeList = _context.SalesEmployeeModel.Where(p => p.Actived == true).Select(p => p.SalesEmployeeCode).ToList();
                    searchViewModel.SalesEmployeeCode = new List<string>();
                    searchViewModel.SalesEmployeeCode.AddRange(salesEmployeeCodeList);
                }

                //Nếu không chọn loại hoạt động nào thì thêm 5 loại hoạt động
                if (searchViewModel.WorkFlowCategoryCode == null || searchViewModel.WorkFlowCategoryCode.Count == 0)
                {
                    searchViewModel.WorkFlowCategoryCode = new List<string>();
                    //searchViewModel.WorkFlowCategoryCode.Add("ACTIVITIES");
                    //searchViewModel.WorkFlowCategoryCode.Add("GT");
                    //searchViewModel.WorkFlowCategoryCode.Add("THKH");
                    //searchViewModel.WorkFlowCategoryCode.Add("TICKET");
                    //searchViewModel.WorkFlowCategoryCode.Add("TICKET_MLC");
                    var workFlowCategoryList = _context.WorkFlowCategoryModel.Where(p => p.ReportType.Contains("CustomerCareActivities"))
                       .Select(p => p.WorkFlowCategoryCode)
                       .ToList();
                    if (workFlowCategoryList != null && workFlowCategoryList.Count > 0)
                    {
                        searchViewModel.WorkFlowCategoryCode.AddRange(workFlowCategoryList);
                    }
                }
                //Nếu không chọn phòng ban thì gửi đi tất cả phòng ban
                if (searchViewModel.DepartmentId == null || searchViewModel.DepartmentId.Count == 0)
                {
                    var departmentList = _context.DepartmentModel.Where(p => p.Actived == true).Select(p => p.DepartmentId).ToList();
                    searchViewModel.DepartmentId = new List<Guid>();
                    searchViewModel.DepartmentId.AddRange(departmentList);
                }
                
                var result = new List<CustomerCareActivitiesViewModel>();
                var data = GetData(searchViewModel);
                //View total
                result = (from p in data
                          group p by new { p.SalesEmployeeName, p.DepartmentName } into gr
                          select new CustomerCareActivitiesViewModel
                          {
                              SalesEmployeeName = gr.Key.SalesEmployeeName,
                              DepartmentName = gr.Key.DepartmentName,
                              QtyGheTham = gr.Sum(p => p.QtyGheTham),
                              QtyTHKH = gr.Sum(p => p.QtyTHKH),
                              QtyBaoHanh = gr.Sum(p => p.QtyBaoHanh),
                              QtyXLKN = gr.Sum(p => p.QtyXLKN),
                              QtyNhiemVu = gr.Sum(p => p.QtyNhiemVu),
                              Total = gr.Sum(p => p.Total)
                          }).OrderBy(p => p.DepartmentName).ThenByDescending(p => p.Total).ToList();
                return _APISuccess(result);
            });
        }

        public List<CustomerCareActivitiesViewModel> GetData(CustomerCareSearchViewModel searchModel)
        {
            try
            {
                var result = new List<CustomerCareActivitiesViewModel>();
                #region SalesEmployeeCode
                //Build your record
                var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

                //And a table as a list of those records
                var tableSalesEmployeeCode = new List<SqlDataRecord>();
                List<string> salesEmployeeCodeLst = new List<string>();
                if (searchModel.SalesEmployeeCode != null && searchModel.SalesEmployeeCode.Count > 0)
                {
                    foreach (var r in searchModel.SalesEmployeeCode)
                    {
                        var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                        tableRow.SetString(0, r);
                        if (!salesEmployeeCodeLst.Contains(r))
                        {
                            salesEmployeeCodeLst.Add(r);
                            tableSalesEmployeeCode.Add(tableRow);
                        }
                    }
                }
                else
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                    tableSalesEmployeeCode.Add(tableRow);
                }
                #endregion

                #region WorkflowCategoryCode
                //Build your record
                //var tableWorkflowCategoryCodeSchema = new List<SqlMetaData>(1)
                //    {
                //        new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                //    }.ToArray();

                //And a table as a list of those records
                var tableWorkflowCategoryCode = new List<SqlDataRecord>();
                List<string> workflowCategoryCodeLst = new List<string>();
                if (searchModel.WorkFlowCategoryCode != null && searchModel.WorkFlowCategoryCode.Count > 0)
                {
                    foreach (var r in searchModel.WorkFlowCategoryCode)
                    {
                        var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                        tableRow.SetString(0, r);
                        if (!workflowCategoryCodeLst.Contains(r))
                        {
                            workflowCategoryCodeLst.Add(r);
                            tableWorkflowCategoryCode.Add(tableRow);
                        }
                    }
                }
                else
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                    tableWorkflowCategoryCode.Add(tableRow);
                }
                #endregion


                #region Department
                //Build your record
                var tableDepartmentSchema = new List<SqlMetaData>(1)
                    {
                        new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                    }.ToArray();

                //And a table as a list of those records
                var tableDepartmentList = new List<SqlDataRecord>();
                //List<Guid> departmentIdLst = new List<Guid>();
                if (searchModel.DepartmentId != null && searchModel.DepartmentId.Count > 0)
                {
                    foreach (var d in searchModel.DepartmentId)
                    {
                        var dId = (Guid)d;
                        var tableRow = new SqlDataRecord(tableDepartmentSchema);
                        tableRow.SetSqlGuid(0, dId);
                        tableDepartmentList.Add(tableRow);

                    }
                }
                else
                {
                    var tableRow = new SqlDataRecord(tableDepartmentSchema);
                    tableDepartmentList.Add(tableRow);
                }
                #endregion

                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Report.usp_CustomerCareCountReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            var tableWork = sda.SelectCommand.Parameters.AddWithValue("@WorkflowCategoryCode", tableWorkflowCategoryCode);
                            tableWork.SqlDbType = SqlDbType.Structured;
                            tableWork.TypeName = "[dbo].[StringList]";
                            var tableSalesEmp = sda.SelectCommand.Parameters.AddWithValue("@SalesEmployeeCode", tableSalesEmployeeCode);
                            tableSalesEmp.SqlDbType = SqlDbType.Structured;
                            tableSalesEmp.TypeName = "[dbo].[StringList]";
                            sda.SelectCommand.Parameters.AddWithValue("@CompanyId", searchModel.CompanyId ?? (object)DBNull.Value);
                            var tableDepartment = sda.SelectCommand.Parameters.AddWithValue("@DepartmentId", tableDepartmentList);
                            tableDepartment.SqlDbType = SqlDbType.Structured;
                            tableDepartment.TypeName = "[dbo].[GuidList]";
                            sda.SelectCommand.Parameters.AddWithValue("@VisitTypeCode", searchModel.VisitTypeCode ?? (object)DBNull.Value);
                            sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.StartFromDate ?? (object)DBNull.Value);
                            sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.StartToDate ?? (object)DBNull.Value);

                            sda.Fill(ds);
                            var dt = ds.Tables[0];

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    var model = new CustomerCareActivitiesViewModel();
                                    model.SalesEmployeeName = item["SalesEmployeeName"].ToString();
                                    model.DepartmentName = item["DepartmentName"].ToString();
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    //ghé thăm
                                    if (!string.IsNullOrEmpty(item["AppointmentQty"].ToString()))
                                    {
                                        model.QtyGheTham = Convert.ToInt32(item["AppointmentQty"].ToString());
                                    }
                                    //THKH
                                    int THKH_SpecKHQty = 0;
                                    if (!string.IsNullOrEmpty(item["THKH_SpecKHQty"].ToString()))
                                    {
                                        THKH_SpecKHQty = Convert.ToInt32(item["THKH_SpecKHQty"].ToString());
                                    }
                                    int THKH_KhaoSatLdDTBQty = 0;
                                    if (!string.IsNullOrEmpty(item["THKH_KhaoSatLdDTBQty"].ToString()))
                                    {
                                        THKH_KhaoSatLdDTBQty = Convert.ToInt32(item["THKH_KhaoSatLdDTBQty"].ToString());
                                    }
                                    int THKH_SpecDTBQty = 0;
                                    if (!string.IsNullOrEmpty(item["THKH_SpecDTBQty"].ToString()))
                                    {
                                        THKH_SpecDTBQty = Convert.ToInt32(item["THKH_SpecDTBQty"].ToString());
                                    }
                                    int THKH_Khac = 0;
                                    if (!string.IsNullOrEmpty(item["THKH_Khac"].ToString()))
                                    {
                                        THKH_Khac = Convert.ToInt32(item["THKH_Khac"].ToString());
                                    }
                                    model.QtyTHKH = THKH_SpecKHQty + THKH_KhaoSatLdDTBQty + THKH_SpecDTBQty + THKH_Khac;
                                    //Bảo hành
                                    //if (!string.IsNullOrEmpty(item["QtyBaoHanh"].ToString()))
                                    //{
                                    //    model.QtyBaoHanh = Convert.ToInt32(item["QtyBaoHanh"].ToString());
                                    //}
                                    //XLKN
                                    int TICKET_XLKNQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_XLKNQty"].ToString()))
                                    {
                                        TICKET_XLKNQty = Convert.ToInt32(item["TICKET_XLKNQty"].ToString());
                                    }
                                    int TICKET_KVQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_KVQty"].ToString()))
                                    {
                                        TICKET_KVQty = Convert.ToInt32(item["TICKET_KVQty"].ToString());
                                    }
                                    int TICKET_KSQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_KSQty"].ToString()))
                                    {
                                        TICKET_KSQty = Convert.ToInt32(item["TICKET_KSQty"].ToString());
                                    }
                                    int TICKET_LDQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_LDQty"].ToString()))
                                    {
                                        TICKET_LDQty = Convert.ToInt32(item["TICKET_LDQty"].ToString());
                                    }
                                    int TICKET_BHQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_BHQty"].ToString()))
                                    {
                                        TICKET_BHQty = Convert.ToInt32(item["TICKET_BHQty"].ToString());
                                    }
                                    int TICKET_HDSDQty = 0;
                                    if (!string.IsNullOrEmpty(item["TICKET_HDSDQty"].ToString()))
                                    {
                                        TICKET_HDSDQty = Convert.ToInt32(item["TICKET_HDSDQty"].ToString());
                                    }
                                    model.QtyXLKN = TICKET_XLKNQty + TICKET_KVQty + TICKET_KSQty + TICKET_BHQty + TICKET_HDSDQty;
                                    //Nhiệm vụ
                                    if (!string.IsNullOrEmpty(item["ActivitiesQty"].ToString()))
                                    {
                                        model.QtyNhiemVu = Convert.ToInt32(item["ActivitiesQty"].ToString());
                                    }
                                    //Giao việc
                                    int MyWorkQty = 0;
                                    if (!string.IsNullOrEmpty(item["MissionQty"].ToString()))
                                    {
                                        MyWorkQty = Convert.ToInt32(item["MissionQty"].ToString());
                                    }
                                    //Tổng cộng
                                    if (!string.IsNullOrEmpty(item["Total"].ToString()))
                                    {
                                        model.Total = Convert.ToInt32(item["Total"].ToString()) - MyWorkQty;
                                    }

                                    result.Add(model);
                                }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion Báo cáo số lượng hoạt động CSKH theo nhân viên

        #region Báo cáo số lượng GVL theo thời gian
        /// <summary>
        /// Báo cáo số lượng góc vật liệu theo thời gian
        /// </summary>
        /// <param name="FromDate_String"></param>
        /// <param name="ToDate_String"></param>
        /// <returns>
        /// 1. STT
        /// 2. Khu vực
        /// 3. SL tổng
        /// 4. SL theo thời điểm
        /// </returns>
        public ActionResult TaskGTBQuantityReport(string FromDate_String, string ToDate_String, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                if (!string.IsNullOrEmpty(FromDate_String))
                {
                    FromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(FromDate_String);
                }
                if (!string.IsNullOrEmpty(ToDate_String))
                {
                    ToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ToDate_String).Value.AddDays(1).AddSeconds(-1); ;
                }
                var query = _unitOfWork.TaskRepository.TaskGTBQuantityReport(FromDate, ToDate);

                #region xử lý kết quả
                var result = new List<TaskGTBQuantityReportViewModel>();

                result = query.ToList();
                
                #endregion

                return _APISuccess(result);
            });
        }
        #endregion Báo cáo số lượng KH showroom
    }
}