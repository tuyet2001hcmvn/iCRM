using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class CustomerCertificateACReportController : BaseController
    {
        // GET: CustomerCertificateACReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (CustomerCertificateACReportSearchViewModel)TempData[CurrentUser.AccountId + "CustomerCertificateACSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerCertificateACTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerCertificateACModeSearch"];
            var pageId = GetPageId("/Reports/CustomerCertificateACReport");
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
            CreateSearchViewBag(searchModel);
            return View(searchModel);
        }

        #region CreateSearchViewBag
        private void CreateSearchViewBag(CustomerCertificateACReportSearchViewModel searchViewModel)
        {
            //Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            //Get phòng ban
            var RolesList = _context.RolesModel.Where(x => x.isEmployeeGroup == true && x.Actived == true).ToList();
            ViewBag.RolesCode = new SelectList(RolesList, "RolesCode", "RolesName");
        }
        #endregion  

        #region Export to Excel
        public ActionResult ExportExcel(CustomerCertificateACReportSearchViewModel searchViewModel)
        {
            DateTime? excelFromDate = searchViewModel.StartFromDate;
            DateTime? excelToDate = searchViewModel.StartToDate;
            DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel, out excelRatioFromDate, out excelRatioToDate);
            return Export(data, filterDisplayList, searchViewModel.StartCommonDate, excelFromDate, excelToDate, excelRatioFromDate, excelRatioToDate);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<CustomerCertificateACReportViewModel> viewModel, List<SearchDisplayModel> filters, string CommonDate = null, DateTime? excelFromDate = null, DateTime? excelToDate = null, DateTime? excelRatioFromDate = null, DateTime? excelRatioToDate = null)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyAppointment", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "NumberOfPrevious", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Ratio", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO LƯỢT TIẾP KHÁCH THEO NHÂN VIÊN";
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
            if (!string.IsNullOrEmpty(CommonDate) && CommonDate != "Custom")
            {
                var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate).ToList();
                var commonDateStr = commonDateList.Where(p => p.CatalogCode == CommonDate).Select(p => p.CatalogText_vi).FirstOrDefault();
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("Tỷ lệ: {0} ({1:dd/MM/yyyy}-{2:dd/MM/yyyy} so với {3:dd/MM/yyyy}-{4:dd/MM/yyyy})", commonDateStr, excelFromDate, excelToDate, excelRatioFromDate, excelRatioToDate),
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = true
                });
            }

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);
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
        private List<CustomerCertificateACReportViewModel> GetData(CustomerCertificateACReportSearchViewModel searchViewModel, out DateTime? excelRatioFromDate, out DateTime? excelRatioToDate)
        {
            excelRatioFromDate = null;
            excelRatioToDate = null;
            if (searchViewModel.StartToDate != null)
            {
                searchViewModel.StartToDate = searchViewModel.StartToDate.Value.AddDays(1).AddSeconds(-1);
            }
            var data = new List<CustomerCertificateACReportViewModel>();
            data = _unitOfWork.CertificateACRepository.GetAllForReport(searchViewModel, CurrentUser.CompanyCode);
            return data;
        }
        #endregion


        public ActionResult ViewDetail(CustomerCertificateACReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerCertificateACSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerCertificateACTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerCertificateACModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CustomerCertificateACReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerCertificateACSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerCertificateACTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerCertificateACModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CustomerCertificateACGridPartial(Guid? templateId = null, CustomerCertificateACReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CustomerCertificateACReport");
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
                return PartialView("_CustomerCertificateACPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CustomerCertificateACReportSearchViewModel>(jsonReq);
                }
                DateTime? excelFromDate = searchViewModel.StartFromDate;
                DateTime? excelToDate = searchViewModel.StartToDate;
                DateTime? excelRatioFromDate, excelRatioToDate;
                var model = GetData(searchViewModel, out excelRatioFromDate, out excelRatioToDate);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerCertificateACPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(CustomerCertificateACReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            DateTime? excelFromDate = searchViewModel.StartFromDate;
            DateTime? excelToDate = searchViewModel.StartToDate;
            DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var model = GetData(searchViewModel, out excelRatioFromDate, out excelRatioToDate);

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

            string fileName = "BAO_CAO_LUOT_TIEP_KHACH_THEO_NHAN_VIEN";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CustomerCertificateACReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.StartFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy") });
            }
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
            if (searchViewModel.ProfileId != null && searchViewModel.ProfileId != Guid.Empty)
            {
                SearchDisplayModel filterPB = new SearchDisplayModel();
                filterPB.DisplayName = "Khách hàng";
                var data = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == searchViewModel.ProfileId);
                filterPB.DisplayValue = data.ProfileName;
                filterList.Add(filterPB);
            }
            return filterList;
        }
        #endregion

        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print(CustomerCertificateACReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Pdf;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }

            DateTime? excelFromDate = searchViewModel.StartFromDate;
            DateTime? excelToDate = searchViewModel.StartToDate;
            DateTime? excelRatioFromDate, excelRatioToDate;

            var model = GetData(searchViewModel, out excelRatioFromDate, out excelRatioToDate);
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