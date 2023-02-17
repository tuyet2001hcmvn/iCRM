using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
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
    public class CustomerHierarchyDetailReportController : BaseController
    {
        // GET: CustomerHierarchyDetailReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (CustomerHierarchyDetailReportSearchViewModel)TempData[CurrentUser.AccountId + "CustomerHierarchyDetailSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerHierarchyDetailTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerHierarchyDetailModeSearch"];
            var pageId = GetPageId("/Reports/CustomerHierarchyDetailReport");
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


        public ActionResult GetProfileForeignCodeSAP(string type, string search)
        {
            var result = new List<ISDSelectItem2>();
            result = (from a in _context.ProfileModel
                      where a.ProfileForeignCode != null 
                      && a.Actived != false
                      && (search == null || a.ProfileForeignCode.Contains(search)
                      || a.ProfileName.Contains(search)
                      || a.ProfileShortName.Contains(search)
                      )
                      orderby a.ProfileForeignCode
                      select new ISDSelectItem2()
                      {
                          value = a.ProfileForeignCode,
                          text = a.ProfileForeignCode + " | " + a.ProfileName
                      }).Take(20).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewDetail(CustomerHierarchyDetailReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CustomerHierarchyDetailReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerHierarchyDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }
        private void CreateSearchViewBag(CustomerHierarchyDetailReportSearchViewModel searchViewModel)
        {
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            //Công ty
            var companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            var rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "1").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var saleOfficeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "3").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var provinceCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "4").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var districtCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "5").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var customerGroupCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "6").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var customerCareerCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "7").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            var groupValueCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "8").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
            if (searchViewModel != null)
            {
                rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "1").Select(x => new { x.Code, Name =  x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();

                //Nhân viên kinh doanh
                var salesEmployeeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "2").Where(x=> searchViewModel.SalesEmployeeCode != null && searchViewModel.SalesEmployeeCode.Contains(x.Code)).Select(x=> new ISDSelectItem2() { value = x.Code, text = x.Code + " | " + x.Description }).ToList();
                ViewBag.SalesEmployeeCodeList = salesEmployeeCode;
                //Khách hàng
                var profileForeignCode = _context.ProfileModel.Select(x=> new { x.ProfileForeignCode, x.ProfileName }).ToList();
                ViewBag.ProfileForeignCodeList = profileForeignCode.Where(a => searchViewModel.ProfileForeignCode != null && searchViewModel.ProfileForeignCode.Contains(a.ProfileForeignCode)).ToList().Select(a => new ISDSelectItem2() { value = a.ProfileForeignCode, text = a.ProfileForeignCode + " | " + a.ProfileName }).ToList();

                saleOfficeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "3").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                provinceCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "4").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                districtCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "5").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                customerGroupCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "6").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                customerCareerCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "7").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                customerCareerCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "7").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x => x.Code).ToList();
                ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);
                groupValueCode = _unitOfWork.SAPReportRepository.GetDanhMuc(CurrentUser.CompanyCode, "8").Select(x => new { x.Code, Name = x.Code + " | " + x.Description }).OrderBy(x=>x.Code).ToList();


            }
            ViewBag.RolesCode = new MultiSelectList(rolesCode, "Code", "Name");
            ViewBag.SaleOfficeCode = new MultiSelectList(saleOfficeCode, "Code", "Name");
            ViewBag.ProvinceCode = new MultiSelectList(provinceCode, "Code", "Name");
            ViewBag.DistrictCode = new MultiSelectList(districtCode, "Code", "Name");
            ViewBag.CustomerGroupCode = new MultiSelectList(customerGroupCode, "Code", "Name");
            ViewBag.CustomerCareerCode = new MultiSelectList(customerCareerCode, "Code", "Name");
            ViewBag.GroupValueCode = new MultiSelectList(groupValueCode, "Code", "Name");

        }

        [ValidateInput(false)]
        public ActionResult CustomerHierarchyDetailPivotGridPartial(Guid? templateId = null, CustomerHierarchyDetailReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CustomerHierarchyDetailReport");
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
                return PartialView("_CustomerHierarchyDetailPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CustomerHierarchyDetailReportSearchViewModel>(jsonReq);
                }
                DateTime? excelFromDate = searchViewModel.FromDate;
                DateTime? excelToDate = searchViewModel.ToDate;
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerHierarchyDetailPivotGridPartial", model);
            }

        }
        private List<SAPGetChiTietDoanhSoViewModel> GetData(CustomerHierarchyDetailReportSearchViewModel searchViewModel)
        {
            List<SAPGetChiTietDoanhSoViewModel> result = new List<SAPGetChiTietDoanhSoViewModel>();
            result = _unitOfWork.SAPReportRepository.GetChiTietDoanhSo(searchViewModel);
            return result;
        }

        [HttpPost]
        public ActionResult ExportPivot(CustomerHierarchyDetailReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            DateTime? excelFromDate = searchViewModel.FromDate;
            DateTime? excelToDate = searchViewModel.ToDate;
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

            string fileName = "BAO_CAO_CHI_TIET_PHAN_CAP_KHACH_HANG";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Export to Excel
        public ActionResult ExportExcel(CustomerHierarchyDetailReportSearchViewModel searchViewModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            return Export(data, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<SAPGetChiTietDoanhSoViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "NumberIndex", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "MaSAP", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Sales", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Adresses", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "GroupEmployee", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO CHI TIẾT PHÂN CẤP KHÁCH HÀNG";
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


        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CustomerHierarchyDetailReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.FromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.FromDate.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.ToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.ToDate.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CompanyCode != null)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công ty";
                //filter.DisplayValue += _context.CompanyModel.FirstOrDefault(s => s.CompanyCode == searchViewModel.CompanyCode).CompanyName;
                filter.DisplayValue += searchViewModel.CompanyCode;
                filterList.Add(filter);
            }
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                foreach (var index in searchViewModel.RolesCode)
                {
                    filter.DisplayValue += _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "1").Where(x=>x.Code == index);
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
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
            if (searchViewModel.CustomerGroupCode != null && searchViewModel.CustomerGroupCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm KH";
                foreach (var code in searchViewModel.CustomerGroupCode)
                {
                    var saleOrg = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code);
                    filter.DisplayValue += saleOrg.CatalogText_vi;
                    filter.DisplayValue += ",";
                }
                filterList.Add(filter);
            }

            return filterList;
        }
        #endregion


        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print (CustomerHierarchyDetailReportSearchViewModel searchViewModel, Guid? templateId)
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