using ISD.Core;
using ISD.Extensions;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class BaoCaoDanhGiaCuaKhachHangController : BaseController
    {
        // GET: BaoCaoDanhGiaCuaKhachHang
        public ActionResult Index()
        {
            var searchModel = (CustomerReviewsSearchModel)TempData[CurrentUser.AccountId + "CustomerReviewsSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerReviewsTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerReviewsModeSearch"];
            //mode search
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
            var pageId = GetPageId("/Reports/BaoCaoDanhGiaCuaKhachHang");
            // search data
            if (searchModel == null || searchModel.IsView != true)
            {
                ViewBag.Search = null;
            }
            else
            {
                ViewBag.Search = searchModel;
            }
            //get list template
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            //get pivot setting
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            //nếu đang có template đang xem
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                //nếu ko có template đang xem thì lấy default của user
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    //nếu user không có template thì lấy default của hệ thống
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else // nếu tất cả đều không có thì render default partial view
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }
            ViewBag.PageId = pageId;
            ViewBag.SystemTemplate = listSystemTemplate;
            ViewBag.UserTemplate = listUserTemplate;
            //CreateViewBag();
            return View();
        }
        public ActionResult ViewDetail(CustomerReviewsSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerReviewsSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerReviewsTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerReviewsModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CustomerReviewsSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerReviewsSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerReviewsTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerReviewsModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CustomerReviewsPivotGridPartial(Guid? templateId = null, CustomerReviewsSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/BaoCaoDanhGiaCuaKhachHang");
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
                return PartialView("_CustomerReviewsPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CustomerReviewsSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerReviewsPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(CustomerReviewsSearchModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
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
            string fileName = "BAO_CAO_DANH_GIA_CUA_KHACH_HANG";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region GetData
        private List<CustomerReviewsReportModel> GetData(CustomerReviewsSearchModel searchModel)
        {
            List<CustomerReviewsReportModel> result = new List<CustomerReviewsReportModel>();
            //fake data 
            //result.Add(new CustomerReviewsReportModel
            //{
            //    CompanyName = "Công Ty Cổ Phần Gỗ An Cường",
            //    DepartmentName = "KHỐI KINH DOANH TIẾP THỊ",
            //    SaleEmployeeName= "Lê Thanh Phong",
            //    VoteQty1 = 1,
            //    VoteQty2 = 2,
            //    VoteQty3 = 3,
            //    VoteQty4 = 4,
            //    VoteQty5 = 5,
            //});
            //result.Add(new CustomerReviewsReportModel
            //{
            //    CompanyName = "Công Ty Cổ Phần Gỗ An Cường",
            //    DepartmentName = "KHỐI KINH DOANH TIẾP THỊ",
            //    SaleEmployeeName = "Nguyễn Thị Mỹ Trinh",
            //    VoteQty1 = 1,
            //    VoteQty2 = 2,
            //    VoteQty3 = 3,
            //    VoteQty4 = 4,
            //    VoteQty5 = 5,
            //});
            //result.Add(new CustomerReviewsReportModel
            //{
            //    CompanyName = "Công Ty Cổ Phần Gỗ An Cường",
            //    DepartmentName = "KHỐI KINH DOANH TIẾP THỊ",
            //    SaleEmployeeName = "Nguyễn Kim Hồng Hạnh",
            //    VoteQty1 = 1,
            //    VoteQty2 = 2,
            //    VoteQty3 = 3,
            //    VoteQty4 = 4,
            //    VoteQty5 = 5,
            //});

            string sqlQuery = "EXEC [Report].[usp_BaoCaoDanhGiaCuaKhachHang] @FromDate, @ToDate";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value,
                },
            };

            result = _context.Database.SqlQuery<CustomerReviewsReportModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }
        #endregion

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CustomerReviewsSearchModel searchViewModel)
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

            return filterList;
        }
        #endregion
    }
}