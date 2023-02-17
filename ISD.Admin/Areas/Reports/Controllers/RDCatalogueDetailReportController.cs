using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class RDCatalogueDetailReportController : BaseController
    {
        // GET: RDCatalogueDetailReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (RDCatalogueDetailSearchViewModel)TempData[CurrentUser.AccountId + "RDCatalogueDetailSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "RDCatalogueDetailTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "RDCatalogueDetailModeSearch"];
            var pageId = GetPageId("/Reports/RDCatalogueDetailReport");
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

            #region CommonDate
            var SelectedCommonDate = "ThisMonth";
            if (searchModel != null)
            {
                SelectedCommonDate = searchModel.CommonDate;
            }
            else
            {
                DateTime? FromDate;DateTime? ToDate;
                _unitOfWork.CommonDateRepository.GetDateBy(SelectedCommonDate, out FromDate, out ToDate);
                searchModel = new RDCatalogueDetailSearchViewModel()
                {
                    CommonDate = SelectedCommonDate,
                    FromDate = FromDate,
                    ToDate = ToDate
                };
            }
            var CurrentDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(CurrentDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            #endregion


            CreateSearchViewBag(searchModel);
            return View(searchModel);
        }

        #region CreateSearchViewBag
        private void CreateSearchViewBag(RDCatalogueDetailSearchViewModel searchViewModel)
        {
            if (searchViewModel == null)
            {
                searchViewModel = new RDCatalogueDetailSearchViewModel();
            }
            //Dropdown Store
            var stockList = _unitOfWork.StockRepository.GetAll();
            ViewBag.StockId = new SelectList(stockList, "StockId", "StockName");


            ViewBag.SearchProductId = new List<ProductViewModel>();
            if (searchViewModel.ProductId != null && searchViewModel.ProductId != Guid.Empty)
            {
                var searchProductList = (from p in _context.ProductModel
                                         where p.Actived == true
                                         && searchViewModel.ProductId == p.ProductId
                                         select new ProductViewModel
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName
                                         }).ToList();
                ViewBag.SearchProductId = searchProductList;
            }
        }
        #endregion  

        #region Export to Excel
        public ActionResult ExportExcel(RDCatalogueDetailSearchViewModel searchViewModel)
        {
            //DateTime? excelFromDate = searchViewModel.StartFromDate;
            //DateTime? excelToDate = searchViewModel.StartToDate;
            //DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);
            
            return Export(data, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<RDCatalogueDetailViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "DocumentType", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "DocumentCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "DocumentDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "Note", isAllowedToEdit = false, isWraptext = true });
            columns.Add(new ExcelTemplate { ColumnName = "TonDauKi", isAllowedToEdit = false, isNumber = true });
            columns.Add(new ExcelTemplate { ColumnName = "ReceiveQty", isAllowedToEdit = false, isNumber = true });
            columns.Add(new ExcelTemplate { ColumnName = "DeliveryQty", isAllowedToEdit = false, isNumber = true });
            columns.Add(new ExcelTemplate { ColumnName = "TonCuoiKi", isAllowedToEdit = false, isNumber = true });
            #endregion Master

            //Header
            string fileheader = LanguageResource.RDCatalogueDetailReport.ToUpper() ;
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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: true, headerRowMergeCount: 1);
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
        private List<RDCatalogueDetailViewModel> GetData(RDCatalogueDetailSearchViewModel searchViewModel)
        {
            //excelRatioFromDate = null;
            //excelRatioToDate = null;
            //if (searchViewModel.StartToDate != null)
            //{
            //    searchViewModel.StartToDate = searchViewModel.StartToDate.Value.AddDays(1).AddSeconds(-1);
            //}
            decimal? TonDauKi; 
            decimal? TonCuoiKi;
            var data = new List<RDCatalogueDetailViewModel>();
            string sqlQuery = "EXEC [Report].[usp_RDCatalogueDetailReport] @StockId, @ProductId, @FromDate, @ToDate, @TonDauKi OUT, @TonCuoiKi OUT";

            var TonDauKiOutParam = new SqlParameter();
            TonDauKiOutParam.ParameterName = "TonDauKi";
            TonDauKiOutParam.SqlDbType = SqlDbType.Decimal;
            TonDauKiOutParam.Direction = ParameterDirection.Output;
            var TonCuoiKiOutParam = new SqlParameter();
            TonCuoiKiOutParam.ParameterName = "TonCuoiKi";
            TonCuoiKiOutParam.SqlDbType = SqlDbType.Decimal;
            TonCuoiKiOutParam.Direction = ParameterDirection.Output;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StockId",
                    Value = searchViewModel.StockId?? (object)DBNull.Value
                },
                 new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductId",
                    Value = searchViewModel.ProductId?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchViewModel.FromDate?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchViewModel.ToDate?? (object)DBNull.Value
                },
                TonDauKiOutParam,
                TonCuoiKiOutParam
            };

            data = _context.Database.SqlQuery<RDCatalogueDetailViewModel>(sqlQuery, parameters.ToArray()).ToList();
            var TonDauKiValue = TonDauKiOutParam.Value;
            if (TonDauKiValue != null)
            {
                TonDauKi = Convert.ToDecimal(TonDauKiValue);
            }
            else
            {
                TonDauKi = 0;
            }
            var TonCuoiKiValue = TonCuoiKiOutParam.Value;
            if (TonCuoiKiValue != null)
            {
                TonCuoiKi = Convert.ToDecimal(TonCuoiKiValue);
            }
            else
            {
                TonCuoiKi = 0;
            }
            
            if (data != null)
            {
                int i = 0;
                foreach (var item in data)
                {
                    i++;
                    item.STT = i;
                    //Ngày đầu: Tồn đầu kì = Tồn đầu kì
                    if (i == 1)
                    {
                        item.TonDauKi = TonDauKi;
                    }
                    else
                    {
                        item.TonDauKi = TonCuoiKi;
                        TonDauKi = item.TonDauKi;
                    }

                    //Tồn cuối kì = Tồn đầu kì + Nhập - Xuất
                    item.TonCuoiKi = TonDauKi + item.ReceiveQty - item.DeliveryQty;
                    
                    
                    TonCuoiKi = item.TonCuoiKi;

                }
            }
            if (data == null)
            {
                data = new List<RDCatalogueDetailViewModel>();
            }
            if (TonDauKiValue != null)
            {
                TonDauKi = Convert.ToDecimal(TonDauKiValue);
            }
            data.Insert(0, new RDCatalogueDetailViewModel { TonDauKi = TonDauKi });
            data.Add(new RDCatalogueDetailViewModel { TonCuoiKi = TonCuoiKi });
            return data;
        }
        #endregion


        public ActionResult ViewDetail(RDCatalogueDetailSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "RDCatalogueDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "RDCatalogueDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "RDCatalogueDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, RDCatalogueDetailSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "RDCatalogueDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "RDCatalogueDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "RDCatalogueDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult RDCatalogueDetailGridPartial(Guid? templateId = null, RDCatalogueDetailSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/RDCatalogueDetailReport");
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
                return PartialView("_RDCatalogueDetailReportPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<RDCatalogueDetailSearchViewModel>(jsonReq);
                }
                //DateTime? excelFromDate = searchViewModel.StartFromDate;
                //DateTime? excelToDate = searchViewModel.StartToDate;
                //DateTime? excelRatioFromDate, excelRatioToDate;
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_RDCatalogueDetailReportPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(RDCatalogueDetailSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            //DateTime? excelFromDate = searchViewModel.StartFromDate;
            //DateTime? excelToDate = searchViewModel.StartToDate;
            //DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var model = GetData(searchViewModel);
            ViewBag.Search = searchViewModel;

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
           
            string fileName = "CHI_TIET_XUAT_NHAP_TON_CATALOGUE";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(RDCatalogueDetailSearchViewModel searchViewModel)
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
            if (searchViewModel.StockId != null &&  searchViewModel.StockId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Kho";
                var sale = _context.StockModel.FirstOrDefault(s => s.StockId == searchViewModel.StockId);
                filter.DisplayValue += sale.StockCode + " | " + sale.StockName;
                filterList.Add(filter);
            }
            if (searchViewModel.ProductId != null && searchViewModel.ProductId != Guid.Empty)
            {
                SearchDisplayModel filterPB = new SearchDisplayModel();
                filterPB.DisplayName = "Catalogue";
                var data = _context.ProductModel.FirstOrDefault(p => p.ProductId == searchViewModel.ProductId);
                filterPB.DisplayValue += data.ProductCode + " | " + data.ProductName;
                filterList.Add(filterPB);
            }
            return filterList;
        }
        #endregion

    }
}