using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ProfileReportController : BaseController
    {
        // GET: ProfileReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index(string Type)
        {
            var searchModel = (ProfileReportPivotSearchViewModel)TempData[CurrentUser.AccountId + "ProfileReportSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ProfileReportTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ProfileReportModeSearch"];
            var parameter = "?Type=" + Type;
            var pageId = GetPageId("/Reports/ProfileReport", parameter);

            if (modeSearch == null || modeSearch.ToString() == "Default")
            {
                ViewBag.ModeSearch = "Default";
            }
            else
            {
                ViewBag.ModeSearch = "Recently";
            }
            Guid templateId = Guid.Empty;
            if (tempalteIdString != null)
            {
                templateId = Guid.Parse(tempalteIdString.ToString());
            }

            if (searchModel == null || searchModel.IsView != true)
            {
                ViewBag.Search = null;
            }
            else
            {
                ViewBag.Search = searchModel;
            }
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }
            ViewBag.PageId = pageId;
            ViewBag.SystemTemplate = listSystemTemplate;
            ViewBag.UserTemplate = listUserTemplate;

            CreateViewBagSearch(searchModel, ProfileType: Type);
            if (Type == ConstProfileType.Account)
            {
                ViewBag.Title = LanguageResource.Reports_ProfileReport;
            }
            else if (Type == ConstProfileType.Contact)
            {
                ViewBag.Title = LanguageResource.Reports_ContactReport;
            }

            //var parameter = "?Type=" + Type;
            ViewBag.PageId = GetPageId("/Reports/ProfileReport", parameter);
            ViewBag.Parameter = parameter;

            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion

            #region CreateRequestTime
            var currentDate = DateTime.Now;
            ViewBag.CreateRequestTimeFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
            ViewBag.CreateRequestTimeTo = currentDate;
            #endregion

            return View(searchModel);
        }

        public ActionResult ViewDetail(ProfileReportPivotSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileReportModeSearch"] = modeSearch;
            return RedirectToAction("Index", new { Type = searchModel.Type });
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProfileReportPivotSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileReportModeSearch"] = modeSearch;
            return RedirectToAction("Index", new { Type = searchModel.Type });
        }

        [ValidateInput(false)]
        public ActionResult ProfilePivotGridPartial(Guid? templateId = null, ProfileReportPivotSearchViewModel searchViewModel = null, string Type = null, string jsonReq = null)
        {
            ViewBag.Type = Type;
            var parameter = "?Type=" + Type;
            var pageId = GetPageId("/Reports/ProfileReport", parameter);
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;

                var Template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
                if (Template != null)
                {
                    ViewBag.LayoutConfigs = Template.LayoutConfigs;
                }
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }

            if ((string.IsNullOrEmpty(jsonReq) || jsonReq == "null") && (searchViewModel == null || searchViewModel.IsView != true))
            {
                ViewBag.Search = null;
                return PartialView("_ProfilePivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProfileReportPivotSearchViewModel>(jsonReq);
                    searchViewModel.ProvinceId = searchViewModel.ProvinceIdList;
                }

                if (templateId.HasValue)
                {
                    var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
                    if (pivotTemplate != null)
                    {
                        searchViewModel.ReportType = pivotTemplate.TemplateName;
                    }
                }

                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ProfilePivotGridPartial", model);
            }
        }

        #region GetData
        private List<ProfileSearchResultViewModel> GetData(ProfileReportPivotSearchViewModel searchViewModel)
        {
            List<ProfileSearchResultViewModel> result = new List<ProfileSearchResultViewModel>();

            int filteredResultsCount;
            //Page Size 
            searchViewModel.PageSize = null;
            //Page Number
            searchViewModel.PageNumber = null;

            searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
            searchViewModel.ProvinceIdList = searchViewModel.ProvinceId;
            searchViewModel.ProvinceId = null;
            #region //Create Date
            if (searchViewModel.CreateCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.CreateFromDate = fromDate;
                searchViewModel.CreateToDate = toDate;
            }
            #endregion

            ProfileRepository repo = new ProfileRepository(_context);
            result = repo.SearchQueryProfileReport(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode);
            //if (result != null && result.Count > 0)
            //{
            //    int i = 0;
            //    foreach (var item in result)
            //    {
            //        i++;
            //        item.STT = i;
            //    }
            //}
            return result;
        }
        #endregion

        [HttpPost]
        public ActionResult _PaggingServerSide(DatatableViewModel model, ProfileSearchViewModel searchViewModel, List<Guid> ProvinceId)
        {
            return ExecuteSearch(() =>
            {
                int filteredResultsCount;
                //10
                int totalResultsCount = model.length;
                //Page Size 
                searchViewModel.PageSize = model.length;
                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;

                searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
                #region //Create Date
                if (searchViewModel.CreateCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.CreateFromDate = fromDate;
                    searchViewModel.CreateToDate = toDate;
                }
                #endregion
                searchViewModel.ProvinceId = null;
                searchViewModel.ProvinceIdList = ProvinceId;
                ProfileRepository repo = new ProfileRepository(_context);

                var profiles = repo.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);
                if (profiles != null && profiles.Count() > 0)
                {
                    int i = 0;
                    foreach (var item in profiles)
                    {
                        i++;
                        item.STT = i;
                        //var ProfileRevenue = _unitOfWork.RevenueRepository.GetProfileRevenueBy(item.ProfileId, "");
                        //item.PreRevenue = ProfileRevenue.Where(p => p.YEARMONTH == DateTime.Now.AddYears(-1).Year.ToString()).Select(p => p.DOANHSO).FirstOrDefault();
                        //item.CurrentRevenue = ProfileRevenue.Where(p => p.YEARMONTH == DateTime.Now.Year.ToString()).Select(p => p.DOANHSO).FirstOrDefault();
                        if (item.Address.StartsWith(","))
                        {
                            item.Address = item.Address.Remove(0, 1).Trim();
                        }
                    }
                }
                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = profiles
                });
            });
        }
        #endregion

        #region CreateViewBag
        public void CreateViewBagSearch(ProfileReportPivotSearchViewModel searchModel = null, string ProfileType = null)
        {
            var _catalogRepository = new CatalogRepository(_context);

            ViewBag.Type = ProfileType;

            #region //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.CatalogCode != ConstCustomerType.Contact
                && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Company and Store
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");
            #endregion

            #region //Get list Age (Độ tuổi)
            var ageList = _catalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region Province (Tỉnh/Thành phố)
            //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
            var provinceAreaList = _context.ProvinceModel.Where(p => p.Actived == true)
                                           .Select(p => new ProvinceViewModel()
                                           {
                                               ProvinceId = p.ProvinceId,
                                               ProvinceCode = p.ProvinceCode,
                                               ProvinceName = p.ProvinceName,
                                               Area = p.Area,
                                               OrderIndex = p.OrderIndex
                                           }).ToList();

            provinceAreaList = provinceAreaList.OrderBy(p => p.ProvinceCode).OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội").ToList();
            ViewBag.ProvinceId = new SelectList(provinceAreaList, "ProvinceId", "ProvinceName");
            #endregion

            #region District (Quận/Huyện)
            ViewBag.DistrictId = new SelectList(new List<DistrictViewModel>(), "DistrictId", "DistrictName");
            if (searchModel != null && searchModel.ProvinceId != null && searchModel.ProvinceId.Count() > 0)
            {
                var districtLst = (from p in _context.DistrictModel
                                   join c in _context.ProvinceModel on p.ProvinceId equals c.ProvinceId
                                   join pr in searchModel.ProvinceId on c.ProvinceId equals pr
                                   where p.Actived == true
                                   && p.ProvinceId != null
                                   orderby c.OrderIndex, p.OrderIndex
                                   select new DistrictViewModel()
                                   {
                                       DistrictId = p.DistrictId,
                                       DistrictName = c.ProvinceName + " | " + p.Appellation + " " + p.DistrictName
                                   }).ToList();
                ViewBag.DistrictId = new MultiSelectList(districtLst, "DistrictId", "DistrictName");
            }
            #endregion

            #region Ward (Phường/Xã)
            ViewBag.WardId = new SelectList(new List<WardViewModel>(), "WardId", "WardName");
            if (searchModel != null && searchModel.DistrictId != null && searchModel.DistrictId.Count() > 0)
            {
                var WardLst = (from p in _context.WardModel
                               join c in _context.DistrictModel on p.DistrictId equals c.DistrictId
                               join pr in searchModel.DistrictId on c.DistrictId equals pr
                               where p.DistrictId != null
                               orderby c.OrderIndex, p.OrderIndex
                               select new WardViewModel()
                               {
                                   WardId = p.WardId,
                                   WardName = c.DistrictName + " | " + p.Appellation + " " + p.WardName
                               }).ToList();
                ViewBag.WardId = new MultiSelectList(WardLst, "WardId", "WardName");
            }
            #endregion

            #region //Get list CustomerCareer (Ngành nghề khách hàng doanh nghiệp)
            var customerCareerList = _context.CatalogModel.Where(
                   p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer
                   && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerCareerCode = new SelectList(customerCareerList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list CustomerGroup (Nhóm khách hàng doanh nghiệp)
            var customerGroupList = _catalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list SalesEmployee (NV phụ trách)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesEmployeeCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            ViewBag.PersonInCharge6Code = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");
            #endregion

            #region //Get list CustomerSource (Nguồn khách hàng)
            //Get AddressType
            var srcLst = _catalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(srcLst, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list CustomerAccountGroup (Phân nhóm khách hàng)
            var customerAccountGroupLst = _catalogRepository.GetCustomerAccountGroup();
            customerAccountGroupLst.Insert(0, new CatalogViewModel()
            {
                CatalogCode = null,
                CatalogText_vi = "Chưa xác định"
            });
            ViewBag.CustomerAccountGroupCode = new SelectList(customerAccountGroupLst, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //isCreateRequest (Yêu cầu tạo khách ở ECC)
            var isCreateRequestLst = new List<ISDSelectBoolItem>();
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "-- Tất cả --",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "Không tạo",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = true,
                name = "Đang yêu cầu",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = false,
                name = "Đã tạo",
            });
            ViewBag.isCreateRequest = new SelectList(isCreateRequestLst, "id", "name");
            #endregion

            #region //Get list SaleOffice (Khu vực)
            var saleOfficeList = _catalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");
            #endregion
            #region //Get list AddressTypeCode (Loại địa chỉ)
            var addressTypeCode = _catalogRepository.GetBy(ConstCatalogType.AddressType);
            ViewBag.AddressTypeCode = new SelectList(addressTypeCode, "CatalogCode", "CatalogText_vi");
            #endregion
            #region Filters
            var filterLst = new List<DropdownlistFilter>();
            //if (ProfileType == ConstProfileType.Account)
            //{
            //    //filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SearchProfileForeignCode, FilterName = LanguageResource.Profile_ProfileForeignCode });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerTypeCode, FilterName = LanguageResource.Profile_CustomerTypeCode });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CompanyId, FilterName = LanguageResource.Profile_CompanyId });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.StoreId, FilterName = LanguageResource.MasterData_Store });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaxNo, FilterName = LanguageResource.Profile_TaxNo });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerAccountGroupCode, FilterName = LanguageResource.Profile_CustomerAccountGroup });
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.isCreateRequest, FilterName = LanguageResource.Profile_isCreateRequest });
            //}
            //else
            //{
            //    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Age, FilterName = LanguageResource.Profile_Age });
            //}
            if (ProfileType == ConstProfileType.Account)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.StoreId, FilterName = LanguageResource.MasterData_Store });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.isCreateRequest, FilterName = LanguageResource.Profile_isCreateRequest });
                //filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaxNo, FilterName = LanguageResource.Profile_TaxNo });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerAccountGroupCode, FilterName = LanguageResource.Profile_CustomerAccountGroup });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CommonCreateDate });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Actived, FilterName = LanguageResource.Actived });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Email, FilterName = LanguageResource.Email });
            }
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerTypeCode, FilterName = LanguageResource.Profile_CustomerTypeCode });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CompanyId, FilterName = LanguageResource.Profile_CompanyId });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.ProvinceId, FilterName = LanguageResource.Profile_ProvinceId });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.DistrictId, FilterName = LanguageResource.Profile_DistrictId });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.WardId, FilterName = LanguageResource.WardId });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.AddressTypeCode, FilterName = LanguageResource.AddressTypeCode });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Address, FilterName = LanguageResource.Profile_Address });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerGroupCode, FilterName = LanguageResource.Profile_CustomerCategoryCode });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerCareerCode, FilterName = LanguageResource.Profile_CustomerCareerCode });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SalesEmployeeCode, FilterName = LanguageResource.PersonInCharge });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.PersonInCharge6Code, FilterName = LanguageResource.PersonInCharge6Code });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.RolesCode, FilterName = LanguageResource.Profile_Department });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerSourceCode, FilterName = LanguageResource.Profile_CustomerSourceCode });

            ViewBag.Filters = filterLst;
            #endregion
        }
        #endregion

        //Export
        #region Export to excel
        const int startIndex = 8;

        public ActionResult ExportExcel(ProfileSearchViewModel searchViewModel, List<Guid> ProvinceId, List<Guid> DistrictId)
        {
            var data = new List<ProfileReportViewModel>();

            int filteredResultsCount;

            #region //Create Date
            if (searchViewModel.CreateCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.CreateFromDate = fromDate;
                searchViewModel.CreateToDate = toDate;
            }
            #endregion

            searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
            searchViewModel.PageNumber = null;
            searchViewModel.PageSize = null;
            searchViewModel.ProvinceId = null;
            searchViewModel.ProvinceIdList = ProvinceId;
            searchViewModel.DistrictId = null;
            searchViewModel.DistrictIdList = DistrictId;


            ProfileRepository repo = new ProfileRepository(_context);
            data = repo.SearchQueryProfileExport(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);

            if (data != null && data.Count() > 0)
            {
                foreach (var item in data)
                {
                    //var ProfileRevenue = _unitOfWork.RevenueRepository.GetProfileRevenueBy(item.ProfileId, "");
                    //item.PreRevenue = ProfileRevenue.Where(p => p.YEARMONTH == DateTime.Now.AddYears(-1).Year.ToString()).Select(p => p.DOANHSO).FirstOrDefault();
                    //item.CurrentRevenue = ProfileRevenue.Where(p => p.YEARMONTH == DateTime.Now.Year.ToString()).Select(p => p.DOANHSO).FirstOrDefault();

                    if (!string.IsNullOrEmpty(item.Address) && item.Address.StartsWith(","))
                    {
                        item.Address = item.Address.Remove(0, 1).Trim();
                    }
                }
            }
            return Export(data, searchViewModel.Type, searchViewModel);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ProfileReportViewModel> viewModel, string Type, ProfileSearchViewModel searchViewModel)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;

            if (Type == ConstProfileType.Account)
            {
                fileheader = "BÁO CÁO TỔNG HỢP KHÁCH HÀNG";
                #region Master
                columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ProfileForeignCode", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Address", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Phone1", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Phone2", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ForeignCustomer", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CustomerSourceName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "SaleOrgName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CustomerTypeName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CustomerGroupName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CustomerCareerName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "WardName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "DistrictName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ProvinceName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "TaxNo", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Age", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Note", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ContactCode", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactPhone", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactEmail", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactPositionName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactDepartmentName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "PersonInCharge", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "RoleInCharge", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false, isDateTimeTime = true });
                columns.Add(new ExcelTemplate { ColumnName = "Actived", isAllowedToEdit = false, isBoolean = true });
                //columns.Add(new ExcelTemplate { ColumnName = "PreRevenue", isAllowedToEdit = false, isCurrency = true });
                //columns.Add(new ExcelTemplate { ColumnName = "CurrentRevenue", isAllowedToEdit = false, isCurrency = true });
                #endregion Master
            }
            else
            {
                fileheader = "BÁO CÁO TỔNG HỢP THÔNG TIN LIÊN HỆ";
                #region Master
                columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ProfileForeignCode", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ForeignCustomer", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "ContactCode", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactPhone", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactPhone1", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactPhone2", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "ContactEmail", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "PositionName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "DepartmentName", isAllowedToEdit = false, isDifferentColorHeader = true, ColorHeader = "#FF6347" });
                columns.Add(new ExcelTemplate { ColumnName = "PersonInCharge", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "RoleInCharge", isAllowedToEdit = false });
                #endregion Master
            }
            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });

            //hiển thị điều kiện lọc
            #region filters
            //Mã CRM
            if (!string.IsNullOrEmpty(searchViewModel.ProfileCode))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Mã CRM", searchViewModel.ProfileCode),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Mã SAP
            if (!string.IsNullOrEmpty(searchViewModel.SearchProfileForeignCode))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Mã SAP", searchViewModel.SearchProfileForeignCode),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Tên KH
            if (!string.IsNullOrEmpty(searchViewModel.ProfileName))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Tên KH", searchViewModel.ProfileName),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //SĐT
            if (!string.IsNullOrEmpty(searchViewModel.Phone))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "SĐT", searchViewModel.Phone),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Phân loại KH
            if (!string.IsNullOrEmpty(searchViewModel.CustomerTypeCode))
            {
                var CustomerType = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerType && p.CatalogCode == searchViewModel.CustomerTypeCode).FirstOrDefault();
                if (CustomerType != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Phân loại KH", CustomerType.CatalogText_vi),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Nguồn khách hàng
            if (!string.IsNullOrEmpty(searchViewModel.CustomerSourceCode))
            {
                var CustomerSource = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerSource && p.CatalogCode == searchViewModel.CustomerSourceCode).FirstOrDefault();
                if (CustomerSource != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Nguồn khách hàng", CustomerSource.CatalogText_vi),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Công ty
            if (searchViewModel.CompanyId.HasValue)
            {
                var company = _context.CompanyModel.Where(p => p.CompanyId == searchViewModel.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Công ty", company.CompanyName),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Chi nhánh
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                var storeLst = _context.StoreModel.Where(p => searchViewModel.StoreId.Contains(p.StoreId)).Select(p => p.StoreName).ToList();
                if (storeLst != null && storeLst.Count > 0)
                {
                    string store = string.Join(", ", storeLst);
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Chi nhánh", store),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Phân nhóm khách hàng
            if (searchViewModel.CustomerAccountGroupCode != null && searchViewModel.CustomerAccountGroupCode.Count > 0)
            {
                var CustomerAccountGroup = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerAccountGroup && searchViewModel.CustomerAccountGroupCode.Contains(p.CatalogCode)).Select(p => p.CatalogText_vi).ToList();
                if (CustomerAccountGroup != null && CustomerAccountGroup.Count > 0)
                {
                    string customerAccountGroup = string.Join(", ", CustomerAccountGroup);
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Phân nhóm khách hàng", customerAccountGroup),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Tỉnh/Thành phố
            if (searchViewModel.ProvinceIdList != null && searchViewModel.ProvinceIdList.Count > 0)
            {
                var ProvinceList = _context.ProvinceModel.Where(p => searchViewModel.ProvinceIdList.Contains(p.ProvinceId)).Select(p => p.ProvinceName).ToList();
                if (ProvinceList != null && ProvinceList.Count > 0)
                {
                    string province = string.Join(", ", ProvinceList);
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Tỉnh/Thành phố", province),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Quận/Huyện
            if (searchViewModel.DistrictIdList.Count > 0)
            {
                var district = _context.DistrictModel.Where(p => searchViewModel.DistrictIdList.Contains(p.DistrictId)).Select(p => p.DistrictName).ToList();
                if (district != null && district.Count > 0)
                {
                    string districtLst = string.Join(", ", district);
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Quận/Huyện", districtLst),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Phường/Xã
            if (searchViewModel.WardId.HasValue)
            {
                var ward = _context.WardModel.Where(p => p.WardId == searchViewModel.WardId).FirstOrDefault();
                if (ward != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Phường/Xã", ward.Appellation + " " + ward.WardName),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Địa chỉ
            if (!string.IsNullOrEmpty(searchViewModel.Address))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Địa chỉ", searchViewModel.Address),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Nhóm khách hàng
            if (searchViewModel.CustomerGroupCode != null && searchViewModel.CustomerGroupCode.Count > 0)
            {
                var CustomerGroup = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerGroup && searchViewModel.CustomerGroupCode.Contains(p.CatalogCode) && p.CatalogText_en.Contains(CurrentUser.CompanyCode)).Select(p => p.CatalogText_vi).ToList();
                if (CustomerGroup != null && CustomerGroup.Count > 0)
                {
                    string customerAccountGroup = string.Join(", ", CustomerGroup);
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Nhóm khách hàng", customerAccountGroup),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Ngành nghề
            if (!string.IsNullOrEmpty(searchViewModel.CustomerCareerCode))
            {
                var CustomerCareer = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer && p.CatalogCode == searchViewModel.CustomerCareerCode).FirstOrDefault();
                if (CustomerCareer != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Ngành nghề", CustomerCareer.CatalogText_vi),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //NV kinh doanh
            if (!string.IsNullOrEmpty(searchViewModel.SalesEmployeeCode))
            {
                var SalesEmployee = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == searchViewModel.SalesEmployeeCode).FirstOrDefault();
                if (SalesEmployee != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "NV kinh doanh", SalesEmployee.SalesEmployeeName),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Phòng ban
            if (!string.IsNullOrEmpty(searchViewModel.RolesCode))
            {
                var roles = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true && p.RolesCode == searchViewModel.RolesCode).FirstOrDefault();
                if (roles != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Phòng ban", roles.RolesName),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //MST
            if (!string.IsNullOrEmpty(searchViewModel.TaxNo))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Mã số thuế", searchViewModel.TaxNo),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Trạng thái
            if (searchViewModel.Actived.HasValue)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Trạng thái", searchViewModel.Actived == true ? LanguageResource.Actived_True : LanguageResource.Actived_False),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Người tạo
            if (!string.IsNullOrEmpty(searchViewModel.CreateByCode))
            {
                var SalesEmployee = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == searchViewModel.CreateByCode).FirstOrDefault();
                if (SalesEmployee != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Người tạo", SalesEmployee.SalesEmployeeName),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }
            }
            //Ngày tạo từ
            if (searchViewModel.CreateFromDate.HasValue)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1:dd/MM/yyyy}", "Ngày tạo từ", searchViewModel.CreateFromDate),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Ngày tạo đến
            if (searchViewModel.CreateToDate.HasValue)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1:dd/MM/yyyy}", "Ngày tạo đến", searchViewModel.CreateToDate),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Email
            if (!string.IsNullOrEmpty(searchViewModel.Email))
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1}", "Email", searchViewModel.Email),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Yêu cầu tạo khách ở ECC
            if (searchViewModel.isCreateRequest.HasValue)
            {
                var isCreateRequestLst = new List<ISDSelectBoolItem>();
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = null,
                    name = "-- Tất cả --",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = null,
                    name = "Không tạo",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = true,
                    name = "Đang yêu cầu",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = false,
                    name = "Đã tạo",
                });
                var currentECCRequest = isCreateRequestLst.Where(p => p.id == searchViewModel.isCreateRequest).FirstOrDefault();
                if (currentECCRequest != null)
                {
                    heading.Add(new ExcelHeadingTemplate()
                    {
                        Content = string.Format("{0}: {1}", "Yêu cầu tạo khách ở ECC", currentECCRequest.name),
                        RowsToIgnore = 0,
                        isWarning = false,
                        isCode = true
                    });
                }

            }
            //Yêu cầu từ ngày
            if (searchViewModel.CreateRequestTimeFrom.HasValue)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1:dd/MM/yyyy}", "Yêu cầu từ ngày", searchViewModel.CreateRequestTimeFrom),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            //Yêu cầu đến ngày
            if (searchViewModel.CreateRequestTimeTo.HasValue)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("{0}: {1:dd/MM/yyyy}", "Yêu cầu đến ngày", searchViewModel.CreateRequestTimeTo),
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            #endregion

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel

        #region Export pivot
        [HttpPost]
        public ActionResult ExportPivot(ProfileReportPivotSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            //var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            //get pivot config 
            //Lấy cột - thứ tự cột ... từ bảng SearchResultDetailTemplateModel
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            //Lấy thông tin config các thông số người dùng lưu template từ SearchResultTemplateModel.LayoutConfigs
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            var headerText = string.Empty;

            if (template != null)
            {
                headerText = template.TemplateName;
            }
            try
            {
                string fileName = "BÁO CÁO TỔNG HỢP KHÁCH HÀNG - " + (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs);

            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProfileReportPivotSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            //hiển thị điều kiện lọc
            #region filters
            //Mã CRM
            if (!string.IsNullOrEmpty(searchViewModel.ProfileCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã CRM";
                var value = searchViewModel.ProfileCode;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Mã SAP
            if (!string.IsNullOrEmpty(searchViewModel.SearchProfileForeignCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã SAP";
                var value = searchViewModel.SearchProfileForeignCode;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Tên KH
            if (!string.IsNullOrEmpty(searchViewModel.ProfileName))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Tên KH";
                var value = searchViewModel.ProfileName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //SĐT
            if (!string.IsNullOrEmpty(searchViewModel.Phone))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "SĐT";
                var value = searchViewModel.Phone;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Phân loại KH
            if (!string.IsNullOrEmpty(searchViewModel.CustomerTypeCode))
            {
                var CustomerType = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerType && p.CatalogCode == searchViewModel.CustomerTypeCode).FirstOrDefault();
                if (CustomerType != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Phân loại KH";
                    var value = CustomerType.CatalogText_vi;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Nguồn khách hàng
            if (!string.IsNullOrEmpty(searchViewModel.CustomerSourceCode))
            {
                var CustomerSource = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerSource && p.CatalogCode == searchViewModel.CustomerSourceCode).FirstOrDefault();
                if (CustomerSource != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Nguồn khách hàng";
                    var value = CustomerSource.CatalogText_vi;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Công ty
            if (searchViewModel.CompanyId.HasValue)
            {
                var company = _context.CompanyModel.Where(p => p.CompanyId == searchViewModel.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Công ty";
                    var value = company.CompanyName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Chi nhánh
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                var storeLst = _context.StoreModel.Where(p => searchViewModel.StoreId.Contains(p.StoreId)).Select(p => p.StoreName).ToList();
                if (storeLst != null && storeLst.Count > 0)
                {
                    string store = string.Join(", ", storeLst);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Chi nhánh";
                    var value = store;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Phân nhóm khách hàng
            if (searchViewModel.CustomerAccountGroupCode != null && searchViewModel.CustomerAccountGroupCode.Count > 0)
            {
                var CustomerAccountGroup = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerAccountGroup && searchViewModel.CustomerAccountGroupCode.Contains(p.CatalogCode)).Select(p => p.CatalogText_vi).ToList();
                if (CustomerAccountGroup != null && CustomerAccountGroup.Count > 0)
                {
                    string customerAccountGroup = string.Join(", ", CustomerAccountGroup);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Phân nhóm khách hàng";
                    var value = customerAccountGroup;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Tỉnh/Thành phố
            if (searchViewModel.ProvinceIdList != null && searchViewModel.ProvinceIdList.Count > 0)
            {
                var ProvinceList = _context.ProvinceModel.Where(p => searchViewModel.ProvinceIdList.Contains(p.ProvinceId)).Select(p => p.ProvinceName).ToList();
                if (ProvinceList != null && ProvinceList.Count > 0)
                {
                    string province = string.Join(", ", ProvinceList);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Tỉnh/Thành phố";
                    var value = province;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Quận/Huyện
            if (searchViewModel.DistrictId != null && searchViewModel.DistrictId.Count > 0)
            {
                var districtLst = _context.DistrictModel.Where(p => searchViewModel.DistrictId.Contains(p.DistrictId)).Select(p => p.Appellation + " " + p.DistrictName).ToList();
                if (districtLst != null && districtLst.Count > 0)
                {
                    string district = string.Join(", ", districtLst);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Quận/Huyện";
                    var value = district;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Phường/Xã
            if (searchViewModel.WardId != null && searchViewModel.WardId.Count > 0)
            {
                var wardLst = _context.WardModel.Where(p => searchViewModel.WardId.Contains(p.WardId)).Select(p => p.Appellation + " " + p.WardName).ToList();

                if (wardLst != null)
                {
                    string ward = string.Join(", ", wardLst);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Phường/Xã";
                    var value = ward;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Địa chỉ
            if (!string.IsNullOrEmpty(searchViewModel.Address))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Địa chỉ";
                var value = searchViewModel.Address;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Nhóm khách hàng
            if (searchViewModel.CustomerGroupCode != null && searchViewModel.CustomerGroupCode.Count() > 0)
            {
                var CustomerGroupLst = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerGroup && searchViewModel.CustomerGroupCode.Contains(p.CatalogCode) && p.CatalogText_en.Contains(CurrentUser.CompanyCode)).Select(x => x.CatalogText_vi).ToList();
                if (CustomerGroupLst != null)
                {
                    var CustomerGroup = string.Join(", ", CustomerGroupLst);
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Nhóm khách hàng";
                    var value = CustomerGroup;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Ngành nghề
            if (!string.IsNullOrEmpty(searchViewModel.CustomerCareerCode))
            {
                var CustomerCareer = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer && p.CatalogCode == searchViewModel.CustomerCareerCode).FirstOrDefault();
                if (CustomerCareer != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Ngành nghề";
                    var value = CustomerCareer.CatalogText_vi;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //NV kinh doanh
            if (!string.IsNullOrEmpty(searchViewModel.SalesEmployeeCode))
            {
                var SalesEmployee = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == searchViewModel.SalesEmployeeCode).FirstOrDefault();
                if (SalesEmployee != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "NV kinh doanh";
                    var value = SalesEmployee.SalesEmployeeName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //NV TVVL
            if (!string.IsNullOrEmpty(searchViewModel.PersonInCharge6Code))
            {
                var SalesEmployee = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == searchViewModel.PersonInCharge6Code).FirstOrDefault();
                if (SalesEmployee != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Nhân viên TVVL";
                    var value = SalesEmployee.SalesEmployeeName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Phòng ban
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count() > 0)
            {
                var roleslst = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true && searchViewModel.RolesCode.Contains(p.RolesCode)).Select(x => x.RolesName).ToList();
                if (roleslst != null)
                {
                    string roles = string.Join(", ", roleslst);
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Phòng ban";
                    var value = roles;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //MST
            if (!string.IsNullOrEmpty(searchViewModel.TaxNo))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã số thuế";
                var value = searchViewModel.TaxNo;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Trạng thái
            if (searchViewModel.Actived.HasValue)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trạng thái";
                var value = searchViewModel.Actived == true ? LanguageResource.Actived_True : LanguageResource.Actived_False;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Người tạo
            if (!string.IsNullOrEmpty(searchViewModel.CreateByCode))
            {
                var SalesEmployee = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == searchViewModel.CreateByCode).FirstOrDefault();
                if (SalesEmployee != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Người tạo";
                    var value = SalesEmployee.SalesEmployeeName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            //Ngày tạo từ
            if (searchViewModel.CreateFromDate.HasValue)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Ngày tạo từ";
                var value = string.Format("{0:dd/MM/yyyy}", searchViewModel.CreateFromDate);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Ngày tạo đến
            if (searchViewModel.CreateToDate.HasValue)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Ngày tạo đến";
                var value = string.Format("{0:dd/MM/yyyy}", searchViewModel.CreateToDate);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Email
            if (!string.IsNullOrEmpty(searchViewModel.Email))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Email";
                var value = searchViewModel.Email;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Yêu cầu tạo khách ở ECC
            if (searchViewModel.isCreateRequest.HasValue)
            {
                var isCreateRequestLst = new List<ISDSelectBoolItem>();
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = null,
                    name = "-- Tất cả --",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = null,
                    name = "Không tạo",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = true,
                    name = "Đang yêu cầu",
                });
                isCreateRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = false,
                    name = "Đã tạo",
                });
                var currentECCRequest = isCreateRequestLst.Where(p => p.id == searchViewModel.isCreateRequest).FirstOrDefault();
                if (currentECCRequest != null)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Yêu cầu tạo khách ở ECC";
                    var value = currentECCRequest.name;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }

            }
            //Yêu cầu từ ngày
            if (searchViewModel.CreateRequestTimeFrom.HasValue)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Yêu cầu từ ngày";
                var value = string.Format("{0}:dd/MM/yyyy}", searchViewModel.CreateRequestTimeFrom);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //Yêu cầu đến ngày
            if (searchViewModel.CreateRequestTimeTo.HasValue)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Yêu cầu đến ngày";
                var value = string.Format("{0}:dd/MM/yyyy}", searchViewModel.CreateRequestTimeTo);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            #endregion
            return filterList;
        }
        #endregion

        public ActionResult GetDistrictByProvince(List<Guid?> ProvinceId)
        {
            var _districtRepository = new DistrictRepository(_context);
            var districtList = new List<DistrictViewModel>();
            if (ProvinceId != null && ProvinceId.Count > 0)
            {
                districtList = (from d in _context.DistrictModel
                                join p in ProvinceId on d.ProvinceId equals p
                                where d.Actived != false
                                orderby d.OrderIndex, d.DistrictName
                                select new DistrictViewModel
                                {
                                    DistrictId = d.DistrictId,
                                    DistrictName = d.Appellation + " " + d.DistrictName
                                }).ToList();
            }
            var districtIdList = new MultiSelectList(districtList, "DistrictId", "DistrictName");
            return Json(districtIdList, JsonRequestBehavior.AllowGet);
        }

        //GetWardByDistrict
        public ActionResult GetWardByDistrict(List<Guid?> DistrictId)
        {
            var _wardRepository = new WardRepository(_context);
            var wardList = new List<WardViewModel>();
            if (DistrictId != null && DistrictId.Count > 0)
            {
                wardList = (from d in _context.WardModel
                            join p in DistrictId on d.DistrictId equals p
                            orderby d.OrderIndex, d.WardName
                            select new WardViewModel
                            {
                                WardId = d.WardId,
                                WardName = d.Appellation + " " + d.WardName
                            }).ToList();
            }

            var wardIdList = new MultiSelectList(wardList, "WardId", "WardName");
            return Json(wardIdList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}