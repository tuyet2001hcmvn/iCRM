using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class NumberOfCustomerBySaleGrowthReportController : BaseController
    {
        // GET: TongHopThongTinDuAn
        public ActionResult Index()
        {
            DateTime? currentFromDate, currentToDate, previousFromDate, previousToDate;
            var CurrentDate = "Custom";
            var PreviousDate = "Custom";
            _unitOfWork.CommonDateRepository.GetDateBy(CurrentDate, out currentFromDate, out currentToDate);
            _unitOfWork.CommonDateRepository.GetDateBy(PreviousDate, out previousFromDate, out previousToDate);
            NumberOfCustomerBySaleGrowthSearchViewModel searchViewModel = new NumberOfCustomerBySaleGrowthSearchViewModel
            {
                CurrentDate = CurrentDate,
                CurrentFromDate = currentFromDate,
                CurrentToDate = currentToDate,
                PreviousDate = PreviousDate,
                PreviousFromDate = previousFromDate,
                PreviousToDate = previousToDate
            };
            var searchModel = (NumberOfCustomerBySaleGrowthSearchViewModel)TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportModeSearch"];
            var pageId = GetPageId("/Reports/NumberOfCustomerBySaleGrowthReport");

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

            var companyCode = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(companyCode, "CompanyCode", "CompanyName", CurrentUser.CompanyCode);
            #region CommonDate
            //var SelectedCommonDate = "";
            //if (searchModel == null)
            //{
            //    SelectedCommonDate = "Custom";
            //}
            ////CurrentDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            #endregion
            //ViewBag.SalesEmployeeCode = new SelectList(salesEmployeeCode, "Code", "Description");
            //ViewBag.SalesEmployeeCode = salesEmployeeCode;
            if (searchModel != null)
            {
                var rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchModel.CompanyCode, "1");
                ViewBag.RolesCode = searchModel.RolesCode;
                ViewBag.RolesName = rolesCode.Where(x=>x.Code == searchModel.RolesCode).Select(x=> x.Code + " | " + x.Description).FirstOrDefault();
                var salesEmployeeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchModel.CompanyCode, "2");
                ViewBag.SalesEmployeeCode = searchModel.SalesEmployeeCode;
                ViewBag.SalesEmployeeName = salesEmployeeCode.Where(x => x.Code == searchModel.SalesEmployeeCode).Select(x => x.Code + " | " + x.Description).FirstOrDefault();

                //Khách hàng
                var profileForeignCode = _context.ProfileModel.Select(x => new { x.ProfileForeignCode, x.ProfileName }).ToList();
                ViewBag.ProfileForeignCodeList = profileForeignCode.Where(a => searchViewModel.ProfileForeignCode != null && searchViewModel.ProfileForeignCode.Contains(a.ProfileForeignCode)).ToList().Select(a => new ISDSelectItem2() { value = a.ProfileForeignCode, text = a.ProfileForeignCode + " | " + a.ProfileName }).ToList();

                //ViewBag.CurrentDate = new SelectList(CurrentDateList, "CatalogCode", "CatalogText_vi", searchModel.CurrentDate);
                //ViewBag.PreviousDate = new SelectList(PreviousDateList, "CatalogCode", "CatalogText_vi", searchModel.PreviousDate);

            }
            return View(searchViewModel);
        }

        public ActionResult GetRolesCodeSAP(string type, string search)
        {
            var rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(type, "1");

            var result = new List<ISDSelectItem2>();
            result = (from a in rolesCode
                      where search == null || a.Code.Contains(search)
                      || a.Description.Contains(search)
                      || a.Content.Contains(search)
                      orderby a.Code
                      select new ISDSelectItem2()
                      {
                          value = a.Code,
                          text = a.Code + " | " + a.Description
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesEmployeeCodeSAP(string type, string search)
        {
            var salesEmployeeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(type, "2");
            var result = new List<ISDSelectItem2>();
            result = (from a in salesEmployeeCode
                      where search == null || a.Code.Contains(search)
                      || a.Description.Contains(search)
                      || a.Content.Contains(search)
                      orderby a.Code
                      select new ISDSelectItem2()
                      {
                          value = a.Code,
                          text = a.Code + " | " + a.Description
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewDetail(NumberOfCustomerBySaleGrowthSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, NumberOfCustomerBySaleGrowthSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "NumberOfCustomerBySaleGrowthReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult NumberOfCustomerBySaleGrowthPivotGridPartial(Guid? templateId = null, NumberOfCustomerBySaleGrowthSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/NumberOfCustomerBySaleGrowthReport");
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
                    if (searchViewModel == null)
                    {
                        searchViewModel = new NumberOfCustomerBySaleGrowthSearchViewModel();
                    }
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
                return PartialView("_NumberOfCustomerBySaleGrowthPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<NumberOfCustomerBySaleGrowthSearchViewModel>(jsonReq);
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
                return PartialView("_NumberOfCustomerBySaleGrowthPivotGridPartial", model);
            }
        }

        #region GetData
        private List<SAPGetTangTruongDoanhSoViewModel> GetData(NumberOfCustomerBySaleGrowthSearchViewModel searchModel)
        {
            List<SAPGetTangTruongDoanhSoViewModel> result = new List<SAPGetTangTruongDoanhSoViewModel>();
            result = _unitOfWork.SAPReportRepository.GetTangTruongDoanhSo(searchModel.CompanyCode, searchModel.RolesCode, searchModel.SalesEmployeeCode, searchModel.CurrentFromDate.Value, searchModel.CurrentToDate.Value, searchModel.PreviousFromDate.Value, searchModel.PreviousToDate.Value);
            return result;
        }
        #endregion

        [HttpPost]
        public ActionResult ExportPivot(NumberOfCustomerBySaleGrowthSearchViewModel searchViewModel, Guid? templateId)
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

            try
            {
                string fileName = (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs);

            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult ExportExcel(NumberOfCustomerBySaleGrowthSearchViewModel searchModel, Guid? templateId)
        {
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchModel.ReportType = pivotTemplate.TemplateName;
            }
            var data = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(data, filterDisplayList, templateId);
        }

        private FileContentResult Export(List<SAPGetTangTruongDoanhSoViewModel> data, List<SearchDisplayModel> filters, Guid? templateId)
        {
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate { ColumnName = "RolesName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileForeignCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Value", isAllowedToEdit = false });
            //Header
            string fileheader = "DANH SÁCH KHÁCH HÀNG THEO TĂNG TRƯỞNG DOANH SỐ";
            
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
            byte[] filecontent = ClassExportExcel.ExportExcel(data, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(NumberOfCustomerBySaleGrowthSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();

            if (searchViewModel.PreviousFromDate != null && searchViewModel.PreviousToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Kỳ trước: ", DisplayValue = "" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày: " + searchViewModel.PreviousFromDate.Value.ToString("dd-MM-yyyy") + "  Đến ngày: " + searchViewModel.PreviousToDate.Value.ToString("dd-MM-yyyy"), DisplayValue = "" });
            }
            if (searchViewModel.CurrentFromDate != null && searchViewModel.CurrentToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Kỳ này: ", DisplayValue = "" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày: "+ searchViewModel.CurrentFromDate.Value.ToString("dd-MM-yyyy") + "  Đến ngày: " + searchViewModel.CurrentToDate.Value.ToString("dd-MM-yyyy"), DisplayValue = "" });
            }
            if (searchViewModel.CompanyCode != null)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công ty";
                filter.DisplayValue += searchViewModel.CompanyCode;
                filterList.Add(filter);
            }
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode != "")
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                filter.DisplayValue += _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "1").Where(x => x.Code == searchViewModel.RolesCode).FirstOrDefault().Description;
                filterList.Add(filter);
            }
            if (searchViewModel.SalesEmployeeCode != null && searchViewModel.SalesEmployeeCode != "")
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên kinh doanh";
                filter.DisplayValue += _unitOfWork.SAPReportRepository.GetDanhMuc(searchViewModel.CompanyCode, "2").Where(x => x.Code == searchViewModel.SalesEmployeeCode).FirstOrDefault().Description;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion

        #region In báo cáo
        [HttpPost]
        public ActionResult Print(NumberOfCustomerBySaleGrowthSearchViewModel searchViewModel, Guid? templateId)
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