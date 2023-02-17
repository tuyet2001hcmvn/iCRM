using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
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
    public class TotalRDCatalogueReportController : BaseController
    {
        // GET: TotalRDCatalogueReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            
            var searchModel = (TotalRDCatalogueReportSearchViewModel)TempData[CurrentUser.AccountId + "TotalRDCatalogueSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "TotalRDCatalogueTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "TotalRDCatalogueModeSearch"];
            var pageId = GetPageId("/Reports/TotalRDCatalogueReport");
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
            if (searchModel == null)
            {
                DateTime? fromDate, toDate;
                var CommonDate = "ThisMonth";
                _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
                searchModel = new TotalRDCatalogueReportSearchViewModel()
                {
                    //Ngày ghé thăm
                    CommonDate = CommonDate,
                    FromDate = fromDate,
                    ToDate = toDate,
                };
            }
            CreateSearchViewBag(searchModel);
            return View(searchModel);
        }

        #region ViewBag
        private void CreateSearchViewBag( TotalRDCatalogueReportSearchViewModel searchViewModel)
        {
            //Showroom (Chi nhánh)
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi",searchViewModel.CommonDate);


            ViewBag.SearchProductId = new List<ProductViewModel>();
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
                ViewBag.SearchProductId = searchProductList;
            }

        }

        
        #endregion

        public ActionResult ViewDetail(TotalRDCatalogueReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TotalRDCatalogueSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TotalRDCatalogueTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TotalRDCatalogueModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TotalRDCatalogueReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TotalRDCatalogueSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TotalRDCatalogueTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TotalRDCatalogueModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult TotalRDCataloguePivotGridPartial(Guid? templateId = null, TotalRDCatalogueReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TotalRDCatalogueReport");
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
                return PartialView("_TotalRDCataloguePivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TotalRDCatalogueReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_TotalRDCataloguePivotGridPartial", model);
            }
        }

        private List<TotalRDCatalogueReportViewModel> GetData(TotalRDCatalogueReportSearchViewModel searchViewModel)
        {
            if (searchViewModel.ToDate != null)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.AddDays(1).AddSeconds(-1);
            }
            var data = new List<TotalRDCatalogueReportViewModel>();
            data = _unitOfWork.AppointmentRepository.GetTotalRDCatalogueReport(searchViewModel, CurrentUser.CompanyCode);
            return data;
        }


        #region Export Pivot
        [HttpPost]
        public ActionResult ExportPivot(TotalRDCatalogueReportSearchViewModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_TONG_HOP_XUAT_NHAP_TON_CATALOGUE";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #endregion

        #region Export Excel
        const int startIndex = 8;
        public ActionResult ExportExcel(TotalRDCatalogueReportSearchViewModel searchViewModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            return Export(data, filterDisplayList);
        }
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<TotalRDCatalogueReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "SaleOrgCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Unit", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TonDauKi", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ReceiveQty", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "DeliveryQty", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TonCuoiKi", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO TỔNG HỢP XUẤT NHẬP TỒN CATALOUGE";
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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TotalRDCatalogueReportSearchViewModel searchViewModel)
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
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chi nhánh";
                foreach (var code in searchViewModel.StoreId)
                {
                    var saleOrg = _context.StoreModel.FirstOrDefault(s => s.StoreId == code);
                    filter.DisplayValue += saleOrg.SaleOrgCode + " | " + saleOrg.StoreName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.ProductId != null && searchViewModel.ProductId.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Catalogue";
                foreach (var code in searchViewModel.ProductId)
                {
                    var prod = _context.ProductModel.FirstOrDefault(s => s.ProductId == code);
                    filter.DisplayValue += prod.ERPProductCode + " | " + prod.ProductName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion


    }
}