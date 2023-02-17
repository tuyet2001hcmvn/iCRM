using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class CostOfUsingCatalogueComparedToApprovalReportController : BaseController
    {
        // GET: CostOfUsingCatalogueComparedToApprovalReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (CostOfUsingCatalogueComparedToApprovalSearchViewModel)TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalModeSearch"];
            var pageId = GetPageId("/Reports/CostOfUsingCatalogueComparedToApprovalReport");
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

            #region CommonDate
            var SelectedCommonDate = "Custom";
            if (searchModel != null)
            {
                SelectedCommonDate = searchModel.CommonDate;
            }
            else
            {
                searchModel = new CostOfUsingCatalogueComparedToApprovalSearchViewModel()
                {
                    CommonDate = SelectedCommonDate
                };
            }
            var CurrentDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(CurrentDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            #endregion


            CreateSearchViewBag(searchModel);
            return View(searchModel);
        }

        #region CreateSearchViewBag
        private void CreateSearchViewBag(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel)
        {
            if (searchViewModel == null)
            {
                searchViewModel = new CostOfUsingCatalogueComparedToApprovalSearchViewModel();
            }
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CurrentUser.CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //Dropdown Department
            var RolesList = _context.RolesModel.Where(x => x.isEmployeeGroup == true && x.Actived == true).ToList();
            ViewBag.RolesCode = new SelectList(RolesList, "RolesCode", "RolesName");
            //Dropdown SaleOfficeCode
            var saleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");


            ViewBag.ProductList = new List<ProductViewModel>();
            if (searchViewModel.ProductId != null && searchViewModel.ProductId.Count() > 0)
            {
                var searchProductList = (from p in _context.ProductModel
                                         where p.Actived == true
                                         && searchViewModel.ProductId.Contains(p.ProductId)
                                         select new ProductViewModel
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName
                                         }).ToList();
                ViewBag.ProductList = searchProductList;
            }
        }
        #endregion  

        #region Export to Excel
        public ActionResult ExportExcel(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel)
        {
            //DateTime? excelFromDate = searchViewModel.StartFromDate;
            //DateTime? excelToDate = searchViewModel.StartToDate;
            //DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            return Export(data, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<CostOfUsingCatalogueComparedToApprovalViewModel> viewModel, List<SearchDisplayModel> filters, string CommonDate = null, DateTime? excelFromDate = null, DateTime? excelToDate = null, DateTime? excelRatioFromDate = null, DateTime? excelRatioToDate = null)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductGroups", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "OfferQuantity", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.Catalogue_Quantity, MergeHeaderTitle = "Đề xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "OfferSampleValueMin", isAllowedToEdit = false, MergeHeaderTitle = "Đề xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "OfferProcessingValueMin", isAllowedToEdit = false, MergeHeaderTitle = "Đề xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "OfferUnitPriceMin", isAllowedToEdit = false, MergeHeaderTitle = "Đề xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "OfferPrice", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.ThanhTien, MergeHeaderTitle = "Đề xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "RequestQuantity", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.Catalogue_Quantity, MergeHeaderTitle = "Được duyệt" });
            columns.Add(new ExcelTemplate { ColumnName = "RequestPrice", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.ThanhTien, MergeHeaderTitle = "Được duyệt" });
            columns.Add(new ExcelTemplate { ColumnName = "TransferredQuantity", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.Catalogue_Quantity, MergeHeaderTitle = "Đã xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "TransferredPrice", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.ThanhTien, MergeHeaderTitle = "Đã xuất" });
            columns.Add(new ExcelTemplate { ColumnName = "DifferenceQuantity", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.Catalogue_Quantity, MergeHeaderTitle = "Chênh lệch" });
            columns.Add(new ExcelTemplate { ColumnName = "DifferencePrice", isAllowedToEdit = false, hasAnotherName = true, AnotherName = LanguageResource.ThanhTien, MergeHeaderTitle = "Chênh lệch" });
            #endregion Master

            //Header
            string fileheader = LanguageResource.CostOfUsingCatalogueComparedToApprovalReport.ToUpper();
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
            #region search info
            foreach (var search in filters)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = search.DisplayName + ": " + search.DisplayValue,
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            #endregion

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: true, headerRowMergeCount: 1);
            //File name
            //Insert => THEM_MOI
            //Edit => CAP_NHAT
            //string exportType = LanguageResource.exportType_Insert;
            //if (isEdit == true)
            //{
            //    exportType = LanguageResource.exportType_Edit;
            //}
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion

        #region GetData for Report
        private List<CostOfUsingCatalogueComparedToApprovalViewModel> GetData(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel)
        {

            if (searchViewModel.ToDate != null)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.AddDays(1).AddSeconds(-1);
            }
            var data = new List<CostOfUsingCatalogueComparedToApprovalViewModel>();
            data = _unitOfWork.CostOfUsingCatalogueComparedToApprovalRepository.GetAllForReport(searchViewModel, CurrentUser.CompanyCode);
            return data;
        }
        #endregion


        public ActionResult ViewDetail(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CostOfUsingCatalogueComparedToApprovalSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CostOfUsingCatalogueComparedToApprovalModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CostOfUsingCatalogueComparedToApprovalGridPartial(Guid? templateId = null, CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CostOfUsingCatalogueComparedToApprovalReport");
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
                return PartialView("_CostOfUsingCatalogueComparedToApprovalPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CostOfUsingCatalogueComparedToApprovalSearchViewModel>(jsonReq);
                }
                //DateTime? excelFromDate = searchViewModel.StartFromDate;
                //DateTime? excelToDate = searchViewModel.StartToDate;
                //DateTime? excelRatioFromDate, excelRatioToDate;

                var model = GetData(searchViewModel);
                foreach (var item in model)
                {
                    if (item.OfferSampleValueMin != item.OfferSampleValueMax && item.OfferSampleValueMax != null)
                    {
                        item.OfferSampleValueMin = item.OfferSampleValueMin - item.OfferSampleValueMax;
                    }

                    if (item.OfferProcessingValueMin != item.OfferProcessingValueMax && item.OfferProcessingValueMax != null)
                    {
                        item.OfferProcessingValueMin = item.OfferProcessingValueMin - item.OfferProcessingValueMax;
                    }
                    
                    if (item.OfferUnitPriceMin != item.OfferUnitPriceMax && item.OfferUnitPriceMax != null)
                    {
                        item.OfferUnitPriceMin = item.OfferUnitPriceMin - item.OfferUnitPriceMax;
                    }
                }

                ViewBag.Search = searchViewModel;
                return PartialView("_CostOfUsingCatalogueComparedToApprovalPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            //DateTime? excelFromDate = searchViewModel.StartFromDate;
            //DateTime? excelToDate = searchViewModel.StartToDate;
            //DateTime? excelRatioFromDate, excelRatioToDate;
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

            string fileName = "CHI_PHI_SU_DUNG_CATALOGUE_SO_VOI_PHE_DUYET";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            //if (searchViewModel.StartFromDate != null)
            //{
            //    filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") });
            //}
            //if (searchViewModel.StartToDate != null)
            //{
            //    filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy") });
            //}
            if (searchViewModel.SalesEmployeeCode != null && searchViewModel.SalesEmployeeCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên kinh doanh";
                foreach (var code in searchViewModel.SalesEmployeeCode)
                {
                    var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == code);
                    filter.DisplayValue += sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.ProductId != null && searchViewModel.ProductId.Count() > 0)
            {
                SearchDisplayModel filterPB = new SearchDisplayModel();
                filterPB.DisplayName = "Catalogue";
                foreach (var item in searchViewModel.ProductId)
                {
                    var data = _context.ProductModel.FirstOrDefault(p => p.ProductId == item);
                    filterPB.DisplayValue += data.ProductCode + " | " + data.ProductName;
                    filterPB.DisplayValue += ", ";
                }
                filterList.Add(filterPB);
            }
            return filterList;
        }
        #endregion

        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Pdf;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            try
            {
                string fileName = (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}