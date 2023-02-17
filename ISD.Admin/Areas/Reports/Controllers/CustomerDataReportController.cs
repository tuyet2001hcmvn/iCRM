using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
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
    public class CustomerDataReportController : BaseController
    {
        // GET: CustomerDataReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (CustomerDataReportSearchModel)TempData[CurrentUser.AccountId + "CustomerDataSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerDataTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerDataModeSearch"];
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
            var pageId = GetPageId("/Reports/CustomerDataReport");
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
            CreateViewBagForSearch();
            return View();
        }
        private void CreateViewBagForSearch()
        {
            //Chi nhánh
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CreateAtSaleOrg = new SelectList(storeList, "StoreId", "StoreName");
            //Phân loại khách hàng
            var catalogList = _context.CatalogModel.Where(
                 p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                 && p.Actived == true && p.CatalogCode != ConstCustomerType.Contact).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");
            //Khu vực
            var SaleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(SaleOfficeList, "CatalogCode", "CatalogText_vi");
            //Tỉnh
            var provinceList = _unitOfWork.ProvinceRepository.GetAll();
            ViewBag.ProvinceId = new MultiSelectList(provinceList, "ProvinceId", "ProvinceName");
            //Quận huyện
            var districtList = _unitOfWork.DistrictRepository.GetAll();
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName");

            //Địa chỉ
            var addressList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.AddressType);
            ViewBag.AddressTypeCode = new SelectList(addressList, "CatalogCode", "CatalogText_vi");
            //Đối tượng
            var ForeignList = new List<SelectListItem>() {
                new SelectListItem() {
                     Text = LanguageResource.Domestic,
                     Value = false.ToString()
                },
                new SelectListItem() {
                     Text = LanguageResource.Foreign,
                     Value = true.ToString()
                }
            };
            ViewBag.IsForeignCustomer = new SelectList(ForeignList, "Value", "Text");
            ViewBag.PageId = GetPageId("/Reports/CustomerDataReport");
        }
        #region export to excel
        public ActionResult ExportExcel(CustomerDataReportSearchModel searchModel)
        {
            var reportData = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(reportData, filterDisplayList);
        }

        private List<CustomerDataReportExcelModel> GetData(CustomerDataReportSearchModel searchModel)
        {
            List<CustomerDataReportExcelModel> result = new List<CustomerDataReportExcelModel>();

            #region ProvinceId
            //Build your record
            var tableProvinceSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableProvince = new List<SqlDataRecord>();
            List<Guid> provinceLst = new List<Guid>();
            if (searchModel.ProvinceId != null && searchModel.ProvinceId.Count > 0)
            {
                foreach (var r in searchModel.ProvinceId)
                {
                    var tableRow = new SqlDataRecord(tableProvinceSchema);
                    tableRow.SetGuid(0, r);
                    if (!provinceLst.Contains(r))
                    {
                        provinceLst.Add(r);
                        tableProvince.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableProvinceSchema);
                tableProvince.Add(tableRow);
            }
            #endregion
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_CustomerDataReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.FromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.ToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@IsForeignCustomer", searchModel.isForeignCustomer ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CustomerTypeCode", searchModel.CustomerTypeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CreateAtSaleOrg", searchModel.CreateAtSaleOrg ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@SaleOfficeCode", searchModel.SaleOfficeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@AddressTypeCode", searchModel.AddressTypeCode ?? (object)DBNull.Value);
                        //sda.SelectCommand.Parameters.AddWithValue("@ProvinceId", searchModel.ProvinceId ?? (object)DBNull.Value);
                        var tableProvinceId = sda.SelectCommand.Parameters.AddWithValue("@ProvinceId", tableProvince);
                        tableProvinceId.SqlDbType = SqlDbType.Structured;
                        tableProvinceId.TypeName = "[dbo].[GuidList]";
                        sda.SelectCommand.Parameters.AddWithValue("@DistrictId", searchModel.DistrictId ?? (object)DBNull.Value);

                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                #region Convert to list
                                CustomerDataReportExcelModel model = new CustomerDataReportExcelModel();
                                model.ProfileForeignCode = item["ProfileForeignCode"].ToString();
                                model.ProfileName = item["ProfileName"].ToString();
                                model.Address = item["Address"].ToString();
                                model.Phone = item["Phone"].ToString();
                                //model.ProductName = item["ProductName"].ToString();
                                //model.SaleOrderCode = item["SaleOrderCode"].ToString();                               
                                model.SaleOfficeName = item["SaleOfficeName"].ToString();
                                model.ProvinceName = item["ProvinceName"].ToString();
                                model.DistrictName = item["DistrictName"].ToString();

                                //FAIL: dòng 2 trở đi gọi SAP không được
                                //if (!string.IsNullOrEmpty(model.ProfileForeignCode))
                                //{
                                //    var saleOrderList = _unitOfWork.SAPReportRepository.GetSaleOrderList(model.ProfileForeignCode, "2000");
                                //    if (saleOrderList != null && saleOrderList.Count > 0)
                                //    {
                                //        model.ProductName = string.Join(", ", saleOrderList.Select(p => p.ProductName).ToArray());
                                //        model.SaleOrderCode = string.Join(", ", saleOrderList.Select(p => p.SONumber).ToArray());
                                //    }
                                //}

                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }
            return result;
        }

        public ActionResult Export(List<CustomerDataReportExcelModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "Báo cáo Dữ liệu khách hàng";

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "ProfileForeignCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false, CustomWidth = 60, isWraptext = true });
            columns.Add(new ExcelTemplate { ColumnName = "Address", isAllowedToEdit = false, CustomWidth = 60, isWraptext = true });
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false, CustomWidth = 60, isWraptext = true });
            columns.Add(new ExcelTemplate { ColumnName = "SaleOrderCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProvinceName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "DistrictName", isAllowedToEdit = false });

            #endregion
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
        public ActionResult ViewDetail(CustomerDataReportSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerDataSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerDataTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerDataModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CustomerDataReportSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerDataSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerDataTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerDataModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CustomerDataPivotGridPartial(Guid? templateId = null, CustomerDataReportSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CustomerDataReport");
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
                return PartialView("_CustomerDataPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CustomerDataReportSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerDataPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(CustomerDataReportSearchModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_DU_LIEU_KHACH_HANG";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CustomerDataReportSearchModel searchViewModel)
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
            if (searchViewModel.isForeignCustomer != null)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Đối tương";
                if (searchViewModel.isForeignCustomer.Value)
                {
                    filter.DisplayValue = LanguageResource.Foreign;
                }
                else
                {
                    filter.DisplayValue = LanguageResource.Domestic;
                }

                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.CustomerTypeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phân loại khách hàng";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.CustomerTypeCode).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.CreateAtSaleOrg))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chi nhánh";
                var id = Guid.Parse(searchViewModel.CreateAtSaleOrg);
                var saleOrg = _context.StoreModel.FirstOrDefault(s => s.StoreId == id);
                filter.DisplayValue = saleOrg.StoreName;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SaleOfficeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khu vực";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.SaleOfficeCode && s.CatalogTypeCode == ConstCatalogType.SaleOffice).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.AddressTypeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Loại địa chỉ";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.AddressTypeCode && s.CatalogTypeCode == ConstCatalogType.AddressType).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.ProvinceId != null && searchViewModel.ProvinceId.Count() > 0)
            {
                var ProvinceList = _context.ProvinceModel.Where(p => searchViewModel.ProvinceId.Contains(p.ProvinceId)).Select(p => p.ProvinceName).ToList();
                if (ProvinceList != null && ProvinceList.Count > 0)
                {
                    string province = string.Join(", ", ProvinceList);

                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Tỉnh/Thành phố";
                    var value = province;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }

            }
            if (searchViewModel.DistrictId != null && searchViewModel.DistrictId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Quận/Huyện";
                var value = _context.DistrictModel.FirstOrDefault(s => s.DistrictId == searchViewModel.DistrictId);
                filter.DisplayValue = value.Appellation + " " + value.DistrictName;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}