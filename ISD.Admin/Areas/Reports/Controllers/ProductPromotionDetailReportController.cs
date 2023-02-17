using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.ViewModels;
using ISD.ViewModels.Reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ProductPromotionDetailReportController : BaseController
    {
        // GET: ProductPromotionDetailReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            ProductPromotionDetailReportSearchViewModel searchViewModel = new ProductPromotionDetailReportSearchViewModel()
            {
                //Ngày ghé thăm
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate,
            };
            var searchModel = (ProductPromotionDetailReportSearchViewModel)TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomSearchData"];
            if (searchModel != null)
            {
                searchViewModel = searchModel;
            }
            
            var tempalteIdString = TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomModeSearch"];
            var pageId = GetPageId("/Reports/ProductPromotionDetailReport");
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
            CreateSearchViewBag(searchViewModel);
            return View(searchViewModel);
        }
        #endregion

        #region CreateViewBag
        private void CreateSearchViewBag(ProductPromotionDetailReportSearchViewModel searchViewModel)
        {
            //Công ty
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");

            //Showroom (Chi nhánh)
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CreateAtSaleOrg = new MultiSelectList(storeList, "SaleOrgCode", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new MultiSelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");
            ViewBag.CheckerCode = new MultiSelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //thời gian
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new MultiSelectList(rolesList, "RolesCode", "RolesName");
            #endregion
        }
        #endregion

        [ValidateInput(false)]
        public ActionResult ProductPromotionDetailReportPivotGridPartial(Guid? templateId = null, ProductPromotionDetailReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ProductPromotionDetailReport");
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
                return PartialView("_ProductPromotionDetailReportPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProductPromotionDetailReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ProductPromotionDetailReportPivotGridPartial", model);
            }
        }

        private List<ProductPromotionDetailReportViewModel> GetData(ProductPromotionDetailReportSearchViewModel searchViewModel)
        {
            if (searchViewModel.ToDate != null)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.AddDays(1).AddSeconds(-1);
            }
            var data = new List<ProductPromotionDetailReportViewModel>();
            data = _unitOfWork.ProductPromotionRepository.GetAllForDetailReport(searchViewModel);
            return data;
        }


        public ActionResult ViewDetail(ProductPromotionDetailReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProductPromotionDetailReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "RateOfAppointmentWithShowRoomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }



        
        #region lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProductPromotionDetailReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.FromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.FromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.ToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.ToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CheckerCode != null && searchViewModel.CheckerCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên";
                var sale = _context.SalesEmployeeModel.Where(s => searchViewModel.CheckerCode.Contains(s.SalesEmployeeCode)).ToList();
                var value = string.Join(", ",sale);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                var sale = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true && searchViewModel.RolesCode.Contains(p.RolesCode)).ToList();
                var value = string.Join(", ",sale);
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion

        [HttpPost]
        public ActionResult ExportPivot(ProductPromotionDetailReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var model = GetData(searchViewModel);
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
            string fileName = "BAO_CAO_TY_LE_NHOM_KHACH_HANG_DEN_SHOWROOM";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }


    }
}