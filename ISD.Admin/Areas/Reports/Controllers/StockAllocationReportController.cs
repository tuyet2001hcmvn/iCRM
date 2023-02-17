using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
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
    public class StockAllocationReportController : BaseController
    {
        // GET: StockAllocationReport
        public ActionResult Index()
        {
            var searchModel = (StockAllocationSearchViewModel)TempData[CurrentUser.AccountId + "StockAllocationSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "StockAllocationTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "StockAllocationModeSearch"];
            var pageId = GetPageId("/Reports/StockAllocationReport");

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
            CreateViewBagForSearch();
            return View();
        }
        private void CreateViewBagForSearch()
        {
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchCompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CurrentUser.CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.SearchStoreId = new SelectList(storeList, "StoreId", "StoreName");        
        }
        public ActionResult ExportExcel(StockAllocationSearchViewModel searchModel)
        {
            searchModel.IsExport = true;
            var result = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            //  int TotalStockDelivery = Convert.ToInt32(result.Sum(p => p.Quantity));
            return Export(result, searchModel.SearchToDate.Value, filterDisplayList);
        }


        public ActionResult Export(List<StockAllocationReportViewModel> viewModel,DateTime toDate,List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO GIÁ TRỊ CATALOGUE PHÂN BỔ THỰC TẾ ĐẾN " + toDate.Date.ToString("dd/MM/yyyy");

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "CompanyName", isAllowedToEdit = false, isDateTime = true }); //1. Công ty   
            columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false, isDateTime = true }); //1. Chi nhánh            
            columns.Add(new ExcelTemplate { ColumnName = "CategoryName", isAllowedToEdit = false });//2. Nhóm vật tư
            //columns.Add(new ExcelTemplate { ColumnName = "RealityQuantity", isAllowedToEdit = false });  //3. Số lượng đã nhập           
            columns.Add(new ExcelTemplate { ColumnName = "ExpectedQuantity", isAllowedToEdit = false, isCurrency = true });//4. Kế hoạch  
            columns.Add(new ExcelTemplate { ColumnName = "TotalValue", isAllowedToEdit = false, isCurrency = true });//5. Thực tế phân bổ
            columns.Add(new ExcelTemplate { ColumnName = "Ratio", isAllowedToEdit = false, isPercent = true });//6. Tỉ lệ       
            #endregion

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
            //heading.Add(new ExcelHeadingTemplate()
            //{
            //    Content = "Tổng số lượng xuất: " + TotalStockDelivery,
            //    RowsToIgnore = 1,
            //    isWarning = false,
            //    isCode = true
            //});

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        public ActionResult ViewDetail(StockAllocationSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockAllocationSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "StockAllocationTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockAllocationModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, StockAllocationSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockAllocationSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "StockAllocationTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockAllocationModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult StockAllocationPivotGridPartial(Guid? templateId = null, StockAllocationSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/StockAllocationReport");
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
                return PartialView("_StockAllocationPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<StockAllocationSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_StockAllocationPivotGridPartial", model);
            }
        }

        private List<StockAllocationReportViewModel> GetData(StockAllocationSearchViewModel searchModel)
        {
            List<StockAllocationReportViewModel> result = new List<StockAllocationReportViewModel>();
            if (!searchModel.SearchToDate.HasValue)
            {
                searchModel.SearchToDate = DateTime.Now;
            }

            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_StockAllocationReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@CompanyId", searchModel.SearchCompanyId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StoreId", searchModel.SearchStoreId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.SearchToDate.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.SearchFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@IsExport", searchModel.IsExport ?? (object)DBNull.Value);
                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];
                        //var filteredResultsCount = output.Value;
                        //if (filteredResultsCount != null)
                        //{
                        //    FilteredResultsCount = Convert.ToInt32(filteredResultsCount);
                        //}

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                #region Convert to list
                                StockAllocationReportViewModel model = new StockAllocationReportViewModel();
                               
                                model.StoreName = item["StoreName"].ToString();
                                if (dt.Columns.Contains("CategoryName"))
                                {
                                    model.CategoryName = item["CategoryName"].ToString();
                                }
                                if (dt.Columns.Contains("CompanyName"))
                                {
                                    model.CompanyName = item["CompanyName"].ToString();
                                }
                                if (!string.IsNullOrEmpty(item["CatalogueRealityQuantity"].ToString()))
                                {
                                    model.CatalogueRealityQuantity = Convert.ToDecimal(item["CatalogueRealityQuantity"].ToString());
                                   
                                }
                                if (!string.IsNullOrEmpty(item["SampleRealityQuantity"].ToString()))
                                {
                                    model.SampleRealityQuantity = Convert.ToDecimal(item["SampleRealityQuantity"].ToString());

                                }
                                if (!string.IsNullOrEmpty(item["KeRealityQuantity"].ToString()))
                                {
                                    model.KeRealityQuantity = Convert.ToDecimal(item["KeRealityQuantity"].ToString());

                                }
                                if (!string.IsNullOrEmpty(item["BrochureRealityQuantity"].ToString()))
                                {
                                    model.BrochureRealityQuantity = Convert.ToDecimal(item["BrochureRealityQuantity"].ToString());

                                }
                                if (!string.IsNullOrEmpty(item["PakingRealityQuantity"].ToString()))
                                {
                                    model.PakingRealityQuantity = Convert.ToDecimal(item["PakingRealityQuantity"].ToString());

                                }
                                if (!string.IsNullOrEmpty(item["ExpectedQuantity"].ToString()))
                                {
                                    model.ExpectedQuantity = Convert.ToDecimal(item["ExpectedQuantity"].ToString());

                                }
                                if (!string.IsNullOrEmpty(item["TotalValue"].ToString()))
                                {
                                    model.TotalValue = Convert.ToDecimal(item["TotalValue"].ToString());
                                }
                                //model.Ratio = ((model.CatalogueRealityQuantity+ model.SampleRealityQuantity+model.KeRealityQuantity+model.BrochureRealityQuantity+model.PakingRealityQuantity) / model.ExpectedQuantity * 100).ToString("##.##");
                                model.Ratio = (model.TotalValue / model.ExpectedQuantity);
                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult ExportPivot(StockAllocationSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
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
            string fileName = "BAO_CAO_SO_LUONG_CATALOGUE_PHAN_BO_THUC_TE";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);

        }
        [HttpPost]
        public ActionResult ExportPivotPDF(StockAllocationSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Pdf;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            string fileName = "BAO_CAO_SO_LUONG_CATALOGUE_PHAN_BO_THUC_TE";
            var s = PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model);
            return s;

        }
       
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(StockAllocationSearchViewModel searchViewModel)
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
                var saleOrg = _context.StoreModel.FirstOrDefault(s => s.StoreId == searchViewModel.SearchStoreId);
                filter.DisplayValue = saleOrg.StoreName;
                filterList.Add(filter);
            }
            if (searchViewModel.SearchFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.SearchFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.SearchToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.SearchToDate.Value.ToString("dd-MM-yyyy") });
            }
            return filterList;
        }
        #endregion
    }
}