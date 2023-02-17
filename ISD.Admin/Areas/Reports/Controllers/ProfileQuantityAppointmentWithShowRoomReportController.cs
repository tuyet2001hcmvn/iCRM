using DevExpress.Web.Mvc;
using DevExpress.Web.Internal;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Reports.Controllers
{
    public class ProfileQuantityAppointmentWithShowRoomReportController : BaseController
    {
        // GET: ProfileQuantityAppointmentWithShowRoomReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (ProfileQuantityAppointmentWithShowRoomReportSearchViewModel)TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomModeSearch"];
            var pageId = GetPageId("/Reports/ProfileQuantityAppointmentWithShowRoomReport");
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
        #endregion Index

        #region CreateSearchViewBag
        private void CreateSearchViewBag(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel)
        {
            //Showroom (Chi nhánh)
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CreateAtSaleOrg = new SelectList(storeList, "SaleOrgCode", "StoreName");

            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            //Nguồn khách hàng
            var customerSources = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSource = new SelectList(customerSources, "CatalogCode", "CatalogText_vi");
        }
        #endregion

        //Export
        #region Export to excel
        //const string controllerCode = ConstExcelController.Appointment;
        const int startIndex = 8;

        public ActionResult ExportExcel(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel)
        {
            DateTime? excelStartFromDate = searchViewModel.StartFromDate;
            DateTime? excelStartToDate = searchViewModel.StartToDate;
            DateTime? excelEndFromDate = searchViewModel.EndFromDate;
            DateTime? excelEndToDate = searchViewModel.EndToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var  data = GetData(searchViewModel);
            return Export(data, filterDisplayList, searchViewModel);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ProfileQuantityAppointmentWithShowRoomReportViewModel> viewModel, List<SearchDisplayModel> filters,  ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel=null)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            //columns.Add(new ExcelTemplate { ColumnName = "ShowroomName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCount", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCountPrevious", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCountDifference", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "Ratio", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO LƯỢT TIẾP KHÁCH THEO SHOWROOM";
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
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = string.Format("Tỷ lệ (kỳ này/Kỳ trước): ({0:dd/MM/yyyy}-{1:dd/MM/yyyy} so với {2:dd/MM/yyyy}-{3:dd/MM/yyyy})", searchViewModel.StartFromDate, searchViewModel.StartToDate, searchViewModel.EndFromDate, searchViewModel.EndToDate),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });

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
        #endregion Export to excel

        
        private List<ProfileQuantityAppointmentWithShowRoomReportViewModel> GetData(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel)
        {
            if (searchViewModel.StartToDate != null)
            {
                searchViewModel.StartToDate = searchViewModel.StartToDate.Value.AddDays(1).AddSeconds(-1);
            }
            var data = new List<ProfileQuantityAppointmentWithShowRoomReportViewModel>();
            data = _unitOfWork.AppointmentRepository.GetProfileQuantityAppointmentWithShowRoomReport(searchViewModel, CurrentUser.CompanyCode, searchViewModel.StartFromDate, searchViewModel.StartToDate);
            if (data != null && data.Count > 0)
            {
                //Lấy dữ liệu kỳ trước
                var previous = _unitOfWork.AppointmentRepository.GetProfileQuantityAppointmentWithShowRoomReport(searchViewModel, CurrentUser.CompanyCode, searchViewModel.EndFromDate, searchViewModel.EndToDate);
                if (previous != null && previous.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var existData = previous.Where(p => p.ShowroomName == item.ShowroomName && p.StoreName == item.StoreName).FirstOrDefault();
                        if (existData != null)
                        {
                            //Kỳ này
                            int KyNay = item.ProfileCount;
                            //Kỳ 
                            int KyTruoc = existData.ProfileCount;
                            //Tỷ lệ kỳ này so với kỳ trước
                            decimal TyLe = 100;
                            if (KyTruoc != 0)
                            {
                                TyLe = ((KyNay - KyTruoc) * 100) / KyNay;
                                //TyLe = KyNay / KyTruoc * 100;
                            }
                            else if (KyTruoc == 0 && KyNay == 0)
                            {
                                TyLe = 0;
                            }
                            else if (KyTruoc == 0 && KyNay != 0)
                            {
                                TyLe = TyLe * KyNay;
                            }
                            item.Ratio = string.Format("{0:n0} %", TyLe);
                            item.ProfileCountPrevious = KyTruoc;
                            item.ProfileCountDifference = KyNay - KyTruoc;
                        }
                        else
                        {
                            item.Ratio = "0 %";
                            item.ProfileCountPrevious = 0;
                        }
                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        //Kỳ này
                        int KyNay = item.ProfileCount;
                        //Kỳ 
                        int KyTruoc = 0;
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
                        item.ProfileCountPrevious = KyTruoc;
                        item.ProfileCountDifference = KyNay - KyTruoc;
                    }
                }
            }
            return data;
        }
        public ActionResult ViewDetail(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileQuantityAppointmentWithShowRoomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ProfileQuantityAppointmentWithShowRoomPivotGridPartial(Guid? templateId = null, ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ProfileQuantityAppointmentWithShowRoomReport");
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
                return PartialView("_ProfileAppointmentShowroomPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProfileQuantityAppointmentWithShowRoomReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                if (searchViewModel.CompareDateTime == true)
                {
                    if (pivotSetting != null)
                    {
                        foreach (var item in pivotSetting)
                        {
                            if (item.FieldName == "ProfileCountPrevious")
                            {
                                if (item.PivotArea == 2 || item.Visible == false)
                                {
                                    item.PivotArea = 3;
                                }

                            }
                        }
                    }

                }
                else
                {
                    if (pivotSetting != null)
                    {
                        foreach (var item in pivotSetting)
                        {
                            if (item.FieldName == "ProfileCountLastYear")
                            {
                                if (item.PivotArea == 2 || item.Visible == false)
                                {
                                    item.PivotArea = 3;
                                }

                            }
                        }
                    }
                }

                return PartialView("_ProfileAppointmentShowroomPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_LUOT_TIEP_KHACH_THEO_SHOWROOM";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel)
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
            if (searchViewModel.CreateAtSaleOrg != null && searchViewModel.CreateAtSaleOrg.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chi nhánh";
                foreach (var code in searchViewModel.CreateAtSaleOrg)
                {
                    var saleOrg = _context.StoreModel.FirstOrDefault(s => s.SaleOrgCode == code);
                    filter.DisplayValue += saleOrg.StoreName;
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
        public ActionResult Print(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchViewModel, Guid? templateId)
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