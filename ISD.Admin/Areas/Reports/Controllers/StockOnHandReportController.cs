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
    public class StockOnHandReportController : BaseController
    {
        // GET: StockOnHandReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (StockOnHandReportSearchModel)TempData[CurrentUser.AccountId + "StockOnHandSearch"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "StockOnHandTempalteId"];
            var modeSearch = TempData[CurrentUser.AccountId + "StockOnHandModeSearch"];
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
            var pageId = GetPageId("/Reports/StockOnHandReport");
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

            CreateViewBagForSearch(searchModel);

            return View();
        }
        private void CreateViewBagForSearch(StockOnHandReportSearchModel searchViewModel)
        {
            if (searchViewModel == null)
            {
                searchViewModel = new StockOnHandReportSearchModel();
            }
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchCompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.SearchStoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.SearchStockId = new SelectList(listStock, "StockId", "StockName");

            // Select multi Product
            ViewBag.SearchProductId = new List<ProductViewModel>();
            if (searchViewModel.SearchProductId != null && searchViewModel.SearchProductId.Count() > 0)
            {
                var searchProductList = (from p in _context.ProductModel
                                         where p.Actived == true
                                         && searchViewModel.SearchProductId.Contains(p.ProductId)
                                         select new ProductViewModel
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName
                                         }).ToList();
                ViewBag.SearchProductId = searchProductList;
            }
        }
        #endregion

        #region ExportExcel
        public ActionResult ExportExcel(StockOnHandReportSearchModel searchModel)
        {
            var result = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(result, filterDisplayList);
        }
        private List<StockOnHandReportViewModel> GetData(StockOnHandReportSearchModel searchModel)
        {
            #region dbo.GuidList searchProductId
            //Build your record
            var tblProductIdSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tblProductId = new List<SqlDataRecord>();
            if (searchModel.SearchProductId != null && searchModel.SearchProductId.Count > 0)
            {
                foreach (var sProductId in searchModel.SearchProductId)
                {
                    var tableRow = new SqlDataRecord(tblProductIdSchema);
                    tableRow.SetSqlGuid(0, sProductId);
                    tblProductId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tblProductIdSchema);
                tblProductId.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_StockOnHandReport] @CompanyId, @StoreId, @StockId, @ProductId";
            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.SearchCompanyId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StoreId",
                    Value = searchModel.SearchStoreId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StockId",
                    Value = searchModel.SearchStockId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    TypeName = "[dbo].[GuidList]",
                    ParameterName = "ProductId",
                    Value = tblProductId
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<StockOnHandReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        public FileContentResult Export(List<StockOnHandReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO TỒN KHO CATALOGUE";
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CategoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StockCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StockName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Qty", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Unit", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "UnitPrice", isAllowedToEdit = false, isCurrency = true });

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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion
        public ActionResult ViewDetail(StockOnHandReportSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockOnHandSearch"] = searchModel;
            TempData[CurrentUser.AccountId + "StockOnHandTempalteId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockOnHandModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, StockOnHandReportSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockOnHandSearch"] = searchModel;
            TempData[CurrentUser.AccountId + "StockOnHandTempalteId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockOnHandModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult StockOnHandPivotGridPartial(Guid? templateId = null, StockOnHandReportSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/StockOnHandReport");
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
                return PartialView("_StockOnHandPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<StockOnHandReportSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_StockOnHandPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(StockOnHandReportSearchModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_TON_KHO_CATALOGUE";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(StockOnHandReportSearchModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.SearchCompanyId != null && searchViewModel.SearchCompanyId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công ty";
                var value = _context.CompanyModel.FirstOrDefault(s => s.CompanyId == searchViewModel.SearchCompanyId).CompanyName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.SearchStoreId != null && searchViewModel.SearchStoreId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chi nhánh";
                var store = _context.StoreModel.FirstOrDefault(s => s.StoreId == searchViewModel.SearchStoreId);
                var value = store.SaleOrgCode + " | " + store.StoreName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.SearchStockId != null && searchViewModel.SearchStockId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Kho";
                var stock = _context.StockModel.FirstOrDefault(s => s.StockId == searchViewModel.SearchStockId);
                var value = stock.StockCode + " | " + stock.StockName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }

            if (searchViewModel.SearchProductId != null && searchViewModel.SearchProductId.Count() > 0)
            {
                var productList = _context.ProductModel.Where(s => searchViewModel.SearchProductId.Contains(s.ProductId)).Select(x => x.ProductName).ToList();
                if (productList != null && productList.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Sản phẩm";
                    var value = string.Join(", ", productList);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            return filterList;
        }
        #endregion
    }
}