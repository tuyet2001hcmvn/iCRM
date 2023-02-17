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
    public class StockTransferReportController : BaseController
    {
        // GET: StockTransferReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (TransferSearchViewModel)TempData[CurrentUser.AccountId + "StockTransferSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "StockTransferTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "StockTransferModeSearch"];
            var pageId = GetPageId("/Reports/StockTransferReport");

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
            CreateViewBagForSearch(searchModel);
            return View();
        }

        #region ViewBag
        private void CreateViewBagForSearch(TransferSearchViewModel searchViewModel)
        {
            if (searchViewModel == null)
            {
                searchViewModel = new TransferSearchViewModel();
            }
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchCompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CurrentUser.CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.SearchStoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SearchSalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.SearchFromStockId = new SelectList(listStock, "StockId", "StockName");
            ViewBag.SearchToStockId = new SelectList(listStock, "StockId", "StockName");

            //Dropdown Department
            var listDepartment = _context.DepartmentModel.Where(s => s.Actived == true);
            ViewBag.SearchDepartmentId = new SelectList(listDepartment, "DepartmentId", "DepartmentName");
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

        #region Export to excel
        public ActionResult ExportExcel(TransferSearchViewModel searchModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            var data = GetData(searchModel);
            //return Export(result, FilteredResultsCount);
            //Tổng số lượng catalog đã xuất
           // int TotalStockDelivery = Convert.ToInt32(result.Sum(p => p.Quantity));
            return Export(data, filterDisplayList);
        }


        public ActionResult Export(List<StockTransferReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO GIAO DỊCH CHUYỂN KHO CATALOGUE";

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "DocumentDate", isAllowedToEdit = false, isDateTime = true }); //1. Ngày chứng từ            
            columns.Add(new ExcelTemplate { ColumnName = "TransferCode", isAllowedToEdit = false });//2. Mã phiếu chuyển          
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });  //3. Mã Catalogue            
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });//4. Tên Catalogue            
            columns.Add(new ExcelTemplate { ColumnName = "CategoryName", isAllowedToEdit = false });//5. Nhóm Vật Tư            
            columns.Add(new ExcelTemplate { ColumnName = "Quantity", isAllowedToEdit = false });//6. Số lượng xuất           
            columns.Add(new ExcelTemplate { ColumnName = "FromStockCode", isAllowedToEdit = false });//7. Mã Kho xuất            
            columns.Add(new ExcelTemplate { ColumnName = "FromStockName", isAllowedToEdit = false });//8. Tên Kho xuất            
            columns.Add(new ExcelTemplate { ColumnName = "ToStockCode", isAllowedToEdit = false });//9. Mã Kho Nhận            
            columns.Add(new ExcelTemplate { ColumnName = "ToStockName", isAllowedToEdit = false });//10. Tên Kho nhận            
            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeName", isAllowedToEdit = false });//11. Nhân viên     
            columns.Add(new ExcelTemplate { ColumnName = "DepartmentName", isAllowedToEdit = false });//Phòng ban     
            columns.Add(new ExcelTemplate { ColumnName = "Note", isAllowedToEdit = false });//12. Ghi chú
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
        #endregion

        private List<StockTransferReportViewModel> GetData(TransferSearchViewModel searchModel)
        {
            List<StockTransferReportViewModel> result = new List<StockTransferReportViewModel>();
            int FilteredResultsCount = 0;
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;


            DataTable dtListProductId = new DataTable();
            dtListProductId.Columns.Add("Id", typeof(Guid));
            if (searchModel.SearchProductId != null)
            {
                foreach (var productId in searchModel.SearchProductId)
                {
                    if (productId != null && productId != Guid.Empty)
                    {
                        dtListProductId.Rows.Add(productId);
                    }
                }
            }

            DataTable dtListFromStockId = new DataTable();
            dtListFromStockId.Columns.Add("Id", typeof(Guid));
            if (searchModel.SearchFromStockId != null)
            {
                foreach (var stockId in searchModel.SearchFromStockId)
                {
                    if (stockId != null && stockId != Guid.Empty)
                    {
                        dtListFromStockId.Rows.Add(stockId);
                    }
                }
            }

            DataTable dtListToStockId = new DataTable();
            dtListToStockId.Columns.Add("Id", typeof(Guid));
            if (searchModel.SearchToStockId != null)
            {
                foreach (var stockId in searchModel.SearchToStockId)
                {
                    if (stockId != null && stockId != Guid.Empty)
                    {
                        dtListToStockId.Rows.Add(stockId);
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_StockTransferReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@CompanyId", searchModel.SearchCompanyId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StoreId", searchModel.SearchStoreId ?? (object)DBNull.Value);
                        var listFromStockIdParameter = new SqlParameter("@FromStockId", SqlDbType.Structured);
                        listFromStockIdParameter.TypeName = "GuidList";
                        listFromStockIdParameter.Value = dtListFromStockId;
                        sda.SelectCommand.Parameters.Add(listFromStockIdParameter);
                        var listToStockIdParameter = new SqlParameter("@ToStockId", SqlDbType.Structured);
                        listToStockIdParameter.TypeName = "GuidList";
                        listToStockIdParameter.Value = dtListToStockId;
                        sda.SelectCommand.Parameters.Add(listToStockIdParameter);
                        sda.SelectCommand.Parameters.AddWithValue("@SalesEmployeeCode", searchModel.SearchSalesEmployeeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@TransferCode", searchModel.SearchTransferCode ?? (object)DBNull.Value);
                        var listProductIdParameter = new SqlParameter("@ProductId", SqlDbType.Structured);
                        listProductIdParameter.TypeName = "GuidList";
                        listProductIdParameter.Value = dtListProductId;
                        sda.SelectCommand.Parameters.Add(listProductIdParameter);
                        sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.SearchFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.SearchToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@isDeleted", searchModel.isDelete ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CurrentCompanyCode", CurrentUser.CompanyCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@DepartmentId", searchModel.SearchDepartmentId ?? (object)DBNull.Value);
                        var output = sda.SelectCommand.Parameters.AddWithValue("@FilteredResultsCount", FilteredResultsCount);
                        output.Direction = ParameterDirection.Output;
                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];
                        var filteredResultsCount = output.Value;
                        if (filteredResultsCount != null)
                        {
                            FilteredResultsCount = Convert.ToInt32(filteredResultsCount);
                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                #region Convert to list
                                StockTransferReportViewModel model = new StockTransferReportViewModel();
                                model.ERPProductCode = item["ERPProductCode"].ToString();
                                // Nhom Catalogue
                                model.ProductGroups = item["ProductGroups"].ToString();
                                model.ProductName = item["ProductName"].ToString();
                                model.CategoryName = item["CategoryName"].ToString();
                                if (!string.IsNullOrEmpty(item["Quantity"].ToString()))
                                {
                                    model.Quantity = Convert.ToDecimal(item["Quantity"].ToString());
                                }
                                model.SalesEmployeeName = item["SalesEmployeeName"].ToString();
                                model.FromStockCode = item["FromStockCode"].ToString();
                                model.FromStockName = item["FromStockName"].ToString();
                                model.ToStockCode = item["ToStockCode"].ToString();
                                model.ToStockName = item["ToStockName"].ToString();
                                model.DepartmentName = item["DepartmentName"].ToString();
                                if (!string.IsNullOrEmpty(item["DocumentDate"].ToString()))
                                {
                                    model.DocumentDate = Convert.ToDateTime(item["DocumentDate"].ToString());
                                }
                                model.TransferCode = item["TransferCode"].ToString();
                                model.Note = item["Note"].ToString();

                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }
            return result;
        }
        public ActionResult ViewDetail(TransferSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockTransferSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "StockTransferTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockTransferModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TransferSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "StockTransferSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "StockTransferTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "StockTransferModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult StockTransferPivotGridPartial(Guid? templateId = null, TransferSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/StockTransferReport");
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
                return PartialView("_StockTransferPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TransferSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_StockTransferPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(TransferSearchViewModel searchViewModel, Guid? templateId)
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
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
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
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TransferSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.SearchFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.SearchFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.SearchToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.SearchToDate.Value.ToString("dd-MM-yyyy") });
            }
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
            if (searchViewModel.SearchFromStockId != null && searchViewModel.SearchFromStockId.Count() > 0)
            {
                var stockLst = _context.StockModel.Where(s => searchViewModel.SearchFromStockId.Contains(s.StockId)).Select(x=> x.StockCode + " | " + x.StockName).ToList();
                if (stockLst != null && stockLst.Count > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Kho xuất";
                    string store = string.Join(", ", stockLst);
                    var value = store;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (searchViewModel.SearchToStockId != null && searchViewModel.SearchToStockId.Count() > 0)
            {
                var stockLst = _context.StockModel.Where(s => searchViewModel.SearchToStockId.Contains(s.StockId)).Select(x => x.StockCode + " | " + x.StockName).ToList();
                if (stockLst != null && stockLst.Count > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Kho nhập";
                    string store = string.Join(", ", stockLst);
                    var value = store;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (searchViewModel.SearchDepartmentId != null && searchViewModel.SearchDepartmentId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                var value = _context.DepartmentModel.FirstOrDefault(s => s.DepartmentId == searchViewModel.SearchDepartmentId).DepartmentName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SearchSalesEmployeeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên";
                var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == searchViewModel.SearchSalesEmployeeCode);
                var value = sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.SearchTransferCode != null)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã phiếu chuyển kho";
                filter.DisplayValue = searchViewModel.SearchTransferCode.ToString();
                filterList.Add(filter);
            }
            if (searchViewModel.SearchProductId != null && searchViewModel.SearchProductId.Count() > 0)
            {
                var productList = _context.ProductModel.Where(s => searchViewModel.SearchProductId.Contains(s.ProductId)).Select(x=>x.ProductName).ToList();
                if (productList != null && productList.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Sản phẩm";
                    var value = string.Join(", ", productList);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (searchViewModel.isDelete != null)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trạng thái";
                if(searchViewModel.isDelete.Value)
                {
                    filter.DisplayValue = "Đã hủy";
                }
                else
                {
                    filter.DisplayValue = "Vừa tạo";
                }               
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion

    }
}