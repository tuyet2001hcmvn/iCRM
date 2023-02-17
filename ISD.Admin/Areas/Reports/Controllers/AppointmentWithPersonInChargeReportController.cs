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
    public class AppointmentWithPersonInChargeReportController : BaseController
    {
        // GET: AppointmentWithPersonInChargeReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (AppointmentWithPersonInChargeReportSearchViewModel)TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeModeSearch"];
            var pageId = GetPageId("/Reports/AppointmentWithPersonInChargeReport");
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
        private void CreateSearchViewBag(AppointmentWithPersonInChargeReportSearchViewModel searchViewModel)
        {
            //Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            //Get phòng ban
           var RolesList = _context.RolesModel.Where(x=>x.isEmployeeGroup == true && x.Actived == true).ToList();
           ViewBag.RolesCode = new SelectList(RolesList, "RolesCode", "RolesName");
        }
        #endregion  

        #region Export to Excel
        public ActionResult ExportExcel(AppointmentWithPersonInChargeReportSearchViewModel searchViewModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            return Export(data, filterDisplayList, searchViewModel.StartCommonDate);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<AppointmentWithPersonInChargeReportViewModel> viewModel, List<SearchDisplayModel> filters, string CommonDate = null)
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
                    Content = search.DisplayName + " " + search.DisplayValue,
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            #endregion
            //if (!string.IsNullOrEmpty(CommonDate) && CommonDate != "Custom")
            //{
            //    var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate).ToList();
            //    var commonDateStr = commonDateList.Where(p => p.CatalogCode == CommonDate).Select(p => p.CatalogText_vi).FirstOrDefault();
            //    heading.Add(new ExcelHeadingTemplate()
            //    {
            //        Content = string.Format("Tỷ lệ: {0}", commonDateStr),
            //        RowsToIgnore = 1,
            //        isWarning = false,
            //        isCode = true
            //    });
            //}

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
        private List<AppointmentWithPersonInChargeReportViewModel> GetData(AppointmentWithPersonInChargeReportSearchViewModel searchViewModel)
        {
            var StartFromDate = searchViewModel.StartFromDate;
            var StartToDate = searchViewModel.StartToDate;
            if (searchViewModel.StartToDate != null)
            {
                searchViewModel.StartToDate = searchViewModel.StartToDate.Value.AddDays(1).AddSeconds(-1);
            }
            if (searchViewModel.EndToDate != null)
            {
                searchViewModel.EndToDate = searchViewModel.EndToDate.Value.AddDays(1).AddSeconds(-1);
            }
            //Lấy dữ liệu kỳ này
            var data = new List<AppointmentWithPersonInChargeReportViewModel>();
            data = _unitOfWork.AppointmentRepository.GetQuantityAppointmentWithPersonInChargeReport(searchViewModel, CurrentUser.CompanyCode, searchViewModel.StartFromDate, searchViewModel.StartToDate);
            if (data != null && data.Count > 0)
            {
                //Lấy dữ liệu kỳ trước
                var previous = _unitOfWork.AppointmentRepository.GetQuantityAppointmentWithPersonInChargeReport(searchViewModel, CurrentUser.CompanyCode, searchViewModel.EndFromDate,  searchViewModel.EndToDate);
                if (previous != null && previous.Count > 0)
                {
                    var stt = 0;
                    foreach (var item in data)
                    {
                        stt++;
                        item.NumberIndex = stt;
                        //Kỳ này
                        decimal KyNay = item.QtyAppointment;
                        decimal KyTruoc = 0;
                        //Tỷ lệ kỳ này so với kỳ trước
                        decimal TyLe = 100;
                        var existData = previous.Where(p => p.SalesEmployeeCode == item.SalesEmployeeCode).FirstOrDefault();
                        if (existData != null)
                        {
                            //Kỳ trước
                            KyTruoc = existData.QtyAppointment;

                            if (KyTruoc != 0)
                            {
                                TyLe = (KyNay - KyTruoc) / KyTruoc * 100;
                            }
                            else if(KyTruoc == 0 && KyNay > 0)
                            {
                                TyLe = KyNay * 100;
                            }
                            else if (KyTruoc == 0 && KyNay == 0)
                            {
                                TyLe = 0;
                            }
                            item.Ratio = string.Format("{0:0.##} %", TyLe);
                            item.NumberOfPrevious = Convert.ToInt32(KyTruoc);
                        }
                        else
                        {
                            TyLe = KyNay * 100;
                            item.Ratio = string.Format("{0:0.##} %", TyLe);
                        }
                    }
                }
                else
                {
                    var stt = 0;
                    foreach (var item in data)
                    {
                        stt++;
                        item.NumberIndex = stt;
                        //Kỳ này
                        int KyNay = item.QtyAppointment;
                        //Kỳ 
                        decimal TyLe = 100;
                        if (KyNay == 0)
                        {
                            TyLe = 0;
                        }
                        else if (KyNay != 0)
                        {
                            TyLe = TyLe * KyNay;
                        }
                        item.Ratio = string.Format("{0:n0} %", TyLe);
                    }
                }
            }
            return data;
        }
        #endregion


        public ActionResult ViewDetail(AppointmentWithPersonInChargeReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, AppointmentWithPersonInChargeReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "AppointmentWithPersonInChargeModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult AppointmentWithPersonInChargeGridPartial(Guid? templateId = null, AppointmentWithPersonInChargeReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/AppointmentWithPersonInChargeReport");
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
                return PartialView("_AppointmentWithPersonInChargePivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<AppointmentWithPersonInChargeReportSearchViewModel>(jsonReq);
                }
                DateTime? excelFromDate = searchViewModel.StartFromDate;
                DateTime? excelToDate = searchViewModel.StartToDate;
                DateTime? excelRatioFromDate, excelRatioToDate;
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_AppointmentWithPersonInChargePivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(AppointmentWithPersonInChargeReportSearchViewModel searchViewModel, Guid? templateId)
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
           
            string fileName = "BAO_CAO_LUOT_TIEP_KHACH_THEO_NHAN_VIEN";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(AppointmentWithPersonInChargeReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.EndFromDate != null && searchViewModel.EndToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Kỳ trước: ", DisplayValue = "" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày: " + searchViewModel.EndFromDate.Value.ToString("dd-MM-yyyy") + "  Đến ngày: " + searchViewModel.EndToDate.Value.ToString("dd-MM-yyyy"), DisplayValue = "" });
            }
            if (searchViewModel.StartFromDate != null && searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Kỳ này: ", DisplayValue = "" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày: " + searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") + "  Đến ngày: " + searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy"), DisplayValue = "" });
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
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                SearchDisplayModel filterPB = new SearchDisplayModel();
                filterPB.DisplayName = "Phòng ban";
                foreach (var items in searchViewModel.RolesCode)
                {
                    var depart = _context.RolesModel.FirstOrDefault(p => p.RolesCode == items && p.isEmployeeGroup == true);
                    filterPB.DisplayValue += depart.RolesName;
                    filterPB.DisplayValue += ", ";
                }
                filterList.Add(filterPB);
            }
            return filterList;
        }
        #endregion

    }
}