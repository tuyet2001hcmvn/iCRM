using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class TicketUsualErrorReportController : BaseController
    {
        // GET: TicketUsualErrorReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            TaskSearchViewModel searchViewModel = new TaskSearchViewModel
            {
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate
            };
            var searchModel = (TaskSearchViewModel)TempData[CurrentUser.AccountId + "TicketUsualErrorSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "TicketUsualErrorTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "TicketUsualErrorModeSearch"];
            var pageId = GetPageId("/Reports/TicketUsualErrorReport");
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
            CreateViewBag(searchModel);
            return View(searchViewModel);
        }
        public void CreateViewBag(TaskSearchViewModel searchViewModel)
        {
            //Mã màu SP
           
            if (searchViewModel == null)
            {
                searchViewModel = new TaskSearchViewModel();
            }
            var productColorLst = new List<ISDSelectItem2>();
            ViewBag.ProductColorList = productColorLst;
            if (searchViewModel.ProductColorCode != null && searchViewModel.ProductColorCode.Count() > 0)
            {
                productColorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductColor)
                                                                .Where(p => searchViewModel.ProductColorCode.Contains(p.CatalogCode))
                                                                .Select(p => new ISDSelectItem2()
                                                                {
                                                                    value = p.CatalogCode,
                                                                    text = p.CatalogCode
                                                                }).ToList();
                ViewBag.ProductColorList = productColorLst;
            }
          

            //(from p in _context.Profile_Opportunity_MaterialModel
            //               join a in _context.View_Product_ProductInfo on p.MaterialId equals a.MaterialId into aG
            //               from prod in aG.DefaultIfEmpty()
            //               where Lists.Contains(p.ReferenceId) && p.MaterialType == ConstMaterialType.ProductColor
            //               select new ISDSelectGuidItem()
            //               {
            //                   id = p.MaterialId.Value,
            //                   name = prod.ProductCode,
            //                   additionalGuid = p.ReferenceId,
            //               }).ToList();




            //Nhóm vật tư
            var productSearchCategoryLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductCategory);
            ViewBag.ProductCategoryCode = new SelectList(productSearchCategoryLst, "CatalogCode", "CatalogText_vi");

            //Các lỗi bảo hành thuờng gặp
            var usualErrorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.UsualError);
            ViewBag.UsualErrorCode = new SelectList(usualErrorLst, "CatalogCode", "CatalogText_vi");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            //Phân cấp SP
            var productlevelCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductLevel);
            ViewBag.ProductLevelCode = new SelectList(productlevelCode, "CatalogCode", "CatalogText_vi");
        }
        #endregion

        

        #region Export Excel
        const int startIndex = 8;

        public ActionResult ExportExcel(TaskSearchViewModel searchViewModel)
        {
            DateTime? excelFromDate = searchViewModel.FromDate;
            DateTime? excelToDate = searchViewModel.ToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            return Export(data, filterDisplayList, searchViewModel.CommonDate, excelFromDate, excelToDate);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<TicketUsualErrorViewModel> viewModel, List<SearchDisplayModel> filters, string CommonDate = null, DateTime? excelFromDate = null, DateTime? excelToDate = null)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "ProductLevelName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductCategoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductColorCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "UsualErrorName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SaleEmployeeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CountOfTaskProduct", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "OrderValue", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "WarrantyValue", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "WarrantyRate", isAllowedToEdit = false});
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO TỔNG HỢP LỖI SẢN PHẨM TRONG XỬ LÝ KHIẾU NẠI";
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

        public ActionResult Export(List<TicketUsualErrorViewModel> viewModel)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO TỔNG HỢP LỖI SẢN PHẨM TRONG XỬ LÝ KHIẾU NẠI";
            columns.Add(new ExcelTemplate { ColumnName = "ProductLevelName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductCategoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductColorCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "UsualErrorName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CountOfTaskProduct", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "WarrantyValue", isAllowedToEdit = false, isCurrency = true });

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

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion


        public ActionResult ViewDetail(TaskSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TicketUsualErrorSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TicketUsualErrorTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TicketUsualErrorModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TaskSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TicketUsualErrorSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TicketUsualErrorTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TicketUsualErrorModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult TicketUsualErrorPivotGridPartial(Guid? templateId = null, TaskSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TicketUsualErrorReport");
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
                return PartialView("_TicketUsualErrorPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TaskSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_TicketUsualErrorPivotGridPartial", model);
            }
        }

        private List<TicketUsualErrorViewModel> GetData(TaskSearchViewModel searchViewModel)
        {
            if (searchViewModel.ToDate != null)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.AddDays(1).AddSeconds(-1);
            }
            List<TicketUsualErrorViewModel> data = _unitOfWork.AppointmentRepository.GetTicketUsualErrorReport(searchViewModel, CurrentUser.CompanyCode);
             return data;
        }

        [HttpPost]
        public ActionResult ExportPivot(TaskSearchViewModel searchViewModel, Guid? pivotTemplate)
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
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(pivotTemplate.Value).ToList();
            //Lấy thông tin config các thông số người dùng lưu template từ SearchResultTemplateModel.LayoutConfigs
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(pivotTemplate.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            var headerText = string.Empty;

            if (template != null)
            {
                headerText = template.TemplateName;
            }
            string fileName = "BAO_CAO_TONG_HOP_LOI_SP_TRONG_XU_LY_KHIEU_NAI";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TaskSearchViewModel searchViewModel)
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
            if (searchViewModel.ProductCategoryCode != null && searchViewModel.ProductCategoryCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm vật tư";
                foreach (var code in searchViewModel.ProductCategoryCode)
                {
                    var saleOrg = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code);
                    filter.DisplayValue += saleOrg.CatalogText_vi;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.UsualErrorCode != null && searchViewModel.UsualErrorCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Các lỗi BH thường gặp";
                foreach (var code in searchViewModel.UsualErrorCode)
                {
                    var saleOrg = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code);
                    filter.DisplayValue += saleOrg.CatalogText_vi;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.ProductColorCode != null && searchViewModel.ProductColorCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Màu sản phẩm";
                foreach (var code in searchViewModel.ProductColorCode)
                {
                    var saleOrg = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code);
                    filter.DisplayValue += saleOrg.CatalogCode;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.ProductLevelCode != null && searchViewModel.ProductLevelCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phân cấp sản phẩm";
                foreach (var code in searchViewModel.ProductLevelCode)
                {
                    var saleOrg = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code);
                    filter.DisplayValue += saleOrg.CatalogText_vi;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }

            return filterList;
        }
        #endregion

        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print(TaskSearchViewModel searchViewModel, Guid? templateId)
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