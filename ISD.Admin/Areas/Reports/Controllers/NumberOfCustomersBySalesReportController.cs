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
    public class NumberOfCustomersBySalesReportController : BaseController
    {
        // GET: TongHopThongTinDuAn
        public ActionResult Index()
        {
            #region Remove sesion
            Session.Remove(CurrentUser.AccountId + "Layout");
            Session.Remove(CurrentUser.AccountId + "LayoutConfigs");
            #endregion

            var searchModel = (NumberOfCustomersBySalesSearchViewModel)TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportModeSearch"];
            var pageId = GetPageId("/Reports/NumberOfCustomersBySalesReport");

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

            //ViewBag.SalesEmployeeCode = new SelectList(salesEmployeeCode, "Code", "Description");
            //ViewBag.SalesEmployeeCode = salesEmployeeCode;
            if (searchModel != null)
            {
                var rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchModel.CompanyCode, "1");
                ViewBag.RolesCode = searchModel.RolesCode;
                ViewBag.RolesName = rolesCode.Where(x => x.Code == searchModel.RolesCode).Select(x => x.Code + " | " + x.Description).FirstOrDefault();
                var salesEmployeeCode = _unitOfWork.SAPReportRepository.GetDanhMuc(searchModel.CompanyCode, "2");
                ViewBag.SalesEmployeeCode = searchModel.SalesEmployeeCode;
                ViewBag.SalesEmployeeName = salesEmployeeCode.Where(x => x.Code == searchModel.SalesEmployeeCode).Select(x => x.Code + " | " + x.Description).FirstOrDefault();
            }
            return View(searchModel);
        }

        public ActionResult GetRolesCodeSAP(string type, string search)
        {
            var rolesCode = _unitOfWork.SAPReportRepository.GetDanhMuc(type, "1");

            var result = new List<ISDSelectItem2>();
            result = (from a in rolesCode
                      where search == null || a.Code.ToLower().Contains(search.ToLower())
                      || a.Description.ToLower().Contains(search.ToLower())
                      || a.Content.ToLower().Contains(search.ToLower())
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
                      where search == null || a.Code.ToLower().Contains(search.ToLower())
                      || a.Description.ToLower().Contains(search.ToLower())
                      || a.Content.ToLower().Contains(search.ToLower())
                      orderby a.Code
                      select new ISDSelectItem2()
                      {
                          value = a.Code,
                          text = a.Code + " | " + a.Description
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewDetail(NumberOfCustomersBySalesSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, NumberOfCustomersBySalesSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "NumberOfCustomersBySalesReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult NumberOfCustomersBySalesPivotGridPartial(Guid? templateId = null, NumberOfCustomersBySalesSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/NumberOfCustomersBySalesReport");
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
                        searchViewModel = new NumberOfCustomersBySalesSearchViewModel();
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
                return PartialView("_NumberOfCustomersBySalesPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<NumberOfCustomersBySalesSearchViewModel>(jsonReq);
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
                return PartialView("_NumberOfCustomersBySalesPivotGridPartial", model);
            }
        }

        #region GetData
        private List<SAPGetPhanCapDoanhSoViewModel> GetData(NumberOfCustomersBySalesSearchViewModel searchModel)
        {
            List<SAPGetPhanCapDoanhSoViewModel> result = new List<SAPGetPhanCapDoanhSoViewModel>();
            result = _unitOfWork.SAPReportRepository.GetPhanCapDoanhSo(searchModel.CompanyCode, searchModel.RolesCode, searchModel.SalesEmployeeCode, searchModel.FromDate, searchModel.ToDate).OrderBy(x => x.ColNo).ToList();
            return result;
        }
        #endregion

        [HttpPost]
        public ActionResult ExportPivot(NumberOfCustomersBySalesSearchViewModel searchViewModel, Guid? templateId)
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
        public ActionResult ExportExcel(NumberOfCustomersBySalesSearchViewModel searchModel, Guid? templateId)
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

        private FileContentResult Export(List<SAPGetPhanCapDoanhSoViewModel> data, List<SearchDisplayModel> filters, Guid? templateId)
        {
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = "Danh sách dự án";

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
        private List<SearchDisplayModel> GetSearchInfoToDisplay(NumberOfCustomersBySalesSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            return filterList;
        }
        #endregion

        #region In báo cáo
        [HttpPost]
        public ActionResult Print(NumberOfCustomersBySalesSearchViewModel searchViewModel, Guid? templateId)
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