using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
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
    public class ProductAccessoryReportController : BaseController
    {
        // GET: ProductAccessoryReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Common Date 2
            var commonDateList2 = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate2 = new SelectList(commonDateList2, "CatalogCode", "CatalogText_vi");

            //ErrorTypeCode
            var errorTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ErrorType);
            ViewBag.ErrorTypeCode = new SelectList(errorTypeLst, "CatalogCode", "CatalogText_vi");

            //ErrorCode
            var errorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Error);
            ViewBag.ErrorCode = new SelectList(errorLst, "CatalogCode", "CatalogText_vi");


            //Nhóm vật tư
            var productSearchCategoryLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductCategory);

            ViewBag.ProductCategoryCode = new SelectList(productSearchCategoryLst, "CatalogCode", "CatalogText_vi");

            //Loại phụ kiện
            var accessoryTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductAccessoryType);
            ViewBag.SearchAccessoryTypeCode = new SelectList(accessoryTypeCode, "CatalogCode", "CatalogText_vi");
            //ViewBag.ProductAccessoryTypeCode = productAccessoryTypeCode;


            var searchModel = (ProductAccessoryReportSearchModel)TempData[CurrentUser.AccountId + "ProductAccessorySearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ProductAccessoryTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ProductAccessoryModeSearch"];
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
            var pageId = GetPageId("/Reports/ProductAccessoryReport");
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
            #region //Get list ServiceTechnicalTeamCode (Trung tâm bảo hành)
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");
            #endregion
            #region //Get list DepartmentCode (Phòng ban)
            var TaskRolesList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true).Where(x => x.RolesCode.Contains(CurrentUser.CompanyCode)).ToList();
            ViewBag.DepartmentCode = new SelectList(TaskRolesList, "RolesCode", "RolesName");
            #endregion
            ViewBag.PageId = pageId;
            ViewBag.SystemTemplate = listSystemTemplate;
            ViewBag.UserTemplate = listUserTemplate;
            return View();
        }

        public ActionResult ExportExcel(ProductAccessoryReportSearchModel searchModel)
        {
            
            if (searchModel.EndCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchModel.EndCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchModel.EndFromDate = fromDate;
                searchModel.EndToDate = toDate;
            }
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            var data = GetData(searchModel);

            return Export(data, filterDisplayList);
        }

        private FileContentResult Export(List<ProductAccessoryReportExcelModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductQuantity", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductCategoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ErrorName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ErrorTypeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ERPAccessoryCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "AccessoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "AccessoryQuantity", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "AccessoryTypeName", isAllowedToEdit = false });

            //Header
            string fileheader = "Báo cáo Tổng hợp sản phẩm phụ kiện bảo hành";

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
    
        private List<ProductAccessoryReportExcelModel> GetData(ProductAccessoryReportSearchModel searchModel)
        {
            if (searchModel.EndCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchModel.EndCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchModel.EndFromDate = fromDate;
                searchModel.EndToDate = toDate;
            }
            #region DepartmentCode
            var tableDepartmentCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableDepartmentCode = new List<SqlDataRecord>();
            List<string> departmentCodeLst = new List<string>();
            if (searchModel.DepartmentCode != null && searchModel.DepartmentCode.Count > 0)
            {
                foreach (var r in searchModel.DepartmentCode)
                {
                    var tableRow = new SqlDataRecord(tableDepartmentCodeSchema);
                    tableRow.SetString(0, r);
                    if (!departmentCodeLst.Contains(r))
                    {
                        departmentCodeLst.Add(r);
                        tableDepartmentCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableDepartmentCodeSchema);
                tableDepartmentCode.Add(tableRow);
            }
            #endregion
            #region ServiceTechnicalTeamCode
            //Build your record
            var tableServiceTechnicalTeamCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableServiceTechnicalTeamCode = new List<SqlDataRecord>();
            List<string> serviceTechnicalTeamCodeLst = new List<string>();
            if (searchModel.ServiceTechnicalTeamCode != null && searchModel.ServiceTechnicalTeamCode.Count > 0)
            {
                foreach (var r in searchModel.ServiceTechnicalTeamCode)
                {
                    var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                    tableRow.SetString(0, r);
                    if (!serviceTechnicalTeamCodeLst.Contains(r))
                    {
                        serviceTechnicalTeamCodeLst.Add(r);
                        tableServiceTechnicalTeamCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                tableServiceTechnicalTeamCode.Add(tableRow);
            }
            #endregion
            var result = new List<ProductAccessoryReportExcelModel>();
            //int FilteredResultsCount = 0;
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_ProductAccessoryReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.EndFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.EndToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ERPProductCode", searchModel.SearchERPProductCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ProductCategoryCode", searchModel.ProductCategoryCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ErrorTypeCode", searchModel.ErrorTypeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ErrorCode", searchModel.ErrorCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ERPAccessoryCode", searchModel.SearchERPAccessoryCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@AccessoryTypeCode", searchModel.SearchAccessoryTypeCode ?? (object)DBNull.Value);
                        //Trung tâm bảo hành
                        var ServiceTechnicalTeamCode = sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", tableServiceTechnicalTeamCode);
                        ServiceTechnicalTeamCode.SqlDbType = SqlDbType.Structured;
                        ServiceTechnicalTeamCode.TypeName = "[dbo].[StringList]";
                        var DepartmentCode = sda.SelectCommand.Parameters.AddWithValue("@DepartmentCode", tableDepartmentCode);
                        DepartmentCode.SqlDbType = SqlDbType.Structured;
                        DepartmentCode.TypeName = "[dbo].[StringList]";
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
                                var model = new ProductAccessoryReportExcelModel();
                                model.ERPProductCode = item["ERPProductCode"].ToString();
                                model.ProductName = item["ProductName"].ToString();
                                if (!string.IsNullOrEmpty(item["ProductQuantity"].ToString()))
                                {
                                    model.ProductQuantity = Convert.ToInt32(item["ProductQuantity"].ToString());
                                }
                                model.ProductCategoryName = item["ProductCategoryName"].ToString();
                                model.ErrorName = item["ErrorName"].ToString();
                                model.ERPAccessoryCode = item["ERPAccessoryCode"].ToString();
                                model.AccessoryName = item["AccessoryName"].ToString();
                                if (!string.IsNullOrEmpty(item["AccessoryQuantity"].ToString()))
                                {
                                    model.AccessoryQuantity = Convert.ToInt32(item["AccessoryQuantity"].ToString());
                                }
                                model.AccessoryTypeName = item["AccessoryTypeName"].ToString();
                                model.ErrorTypeName = item["ErrorTypeName"].ToString();
                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }

            return result;

        }
        public ActionResult ViewDetail(ProductAccessoryReportSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProductAccessorySearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProductAccessoryTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProductAccessoryModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProductAccessoryReportSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProductAccessorySearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProductAccessoryTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProductAccessoryModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ProductAccessoryPivotGridPartial(Guid? templateId = null, ProductAccessoryReportSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ProductAccessoryReport");
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
                return PartialView("_ProductAccessoryPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProductAccessoryReportSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ProductAccessoryPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ProductAccessoryReportSearchModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_TONG_HOP_SAN_PHAM_PHU_KIEN_BAO_HANH";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProductAccessoryReportSearchModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.EndFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.EndFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.EndToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.EndToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (!string.IsNullOrEmpty(searchViewModel.SearchERPProductCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã SAP sản phẩm";
                var value = searchViewModel.SearchERPProductCode;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.ProductCategoryCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phân loại sản phẩn";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.ProductCategoryCode && s.CatalogTypeCode== ConstCatalogType.ProductCategory).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }

            if (searchViewModel.ServiceTechnicalTeamCode != null && searchViewModel.ServiceTechnicalTeamCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trung tâm bảo hành";
                var listCatalog = _context.CatalogModel.Where(s => s.CatalogTypeCode == ConstCatalogType.ServiceTechnicalTeam).ToList();
                var value = listCatalog.Where(x => searchViewModel.ServiceTechnicalTeamCode.Contains(x.CatalogCode)).Select(x => x.CatalogText_vi).ToList();
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.ErrorTypeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Hình thức bảo hành";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.ErrorTypeCode && s.CatalogTypeCode == ConstCatalogType.ErrorType).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.ErrorCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phương thức xử lý";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.ErrorCode && s.CatalogTypeCode == ConstCatalogType.Error).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SearchERPAccessoryCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã SAP phụ kiện";
                var value = searchViewModel.SearchERPAccessoryCode;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SearchAccessoryTypeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phân loại phụ kiện";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.SearchAccessoryTypeCode && s.CatalogTypeCode == ConstCatalogType.ProductAccessoryType).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}