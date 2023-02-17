using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Excel;
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
    public class CustomerAmountByCRMController : BaseController
    {
        // GET: CustomerAmountByCRM
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (CustomerAmountByCRMSearchViewModel)TempData[CurrentUser.AccountId + "CustomerAmountByCRMSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerAmountByCRMTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerAmountByCRMModeSearch"];
            var pageId = GetPageId("/Reports/CustomerAmountByCRM");

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
           

          
            CreateViewBag();

            return View();
        }

        public ActionResult ExportExcel(CustomerAmountByCRMSearchViewModel searchViewModel)
        {
            //DateTime? excelFromDate = searchViewModel.FromDate;
            // DateTime? excelToDate = searchViewModel.ToDate;
            // DateTime? excelRatioFromDate, excelRatioToDate;
            var data = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return Export(data, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<CustomerAmountByCRMViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            if (viewModel != null && viewModel.Count() > 0)
            {
                decimal? sumQtyEccB = viewModel.Sum(x => x.QtyECCB);
                decimal? sumQtyCRMB = viewModel.Sum(x => x.QtyCRMB);
                decimal? sumQtyTotalB = viewModel.Sum(x => x.QtyTotalB);
                decimal? sumQtyEccC = viewModel.Sum(x => x.QtyECCC);
                decimal? sumQtyCRMC = viewModel.Sum(x => x.QtyCRMC);
                decimal? sumQtyTotalC = viewModel.Sum(x => x.QtyTotalC);
                decimal? sumQtyEcc = viewModel.Sum(x => x.QtyECC);
                decimal? sumQtyCRM = viewModel.Sum(x => x.QtyCRM);
                decimal? sumQtyTotal = viewModel.Sum(x => x.QtyTotal);
                CustomerAmountByCRMViewModel total = new CustomerAmountByCRMViewModel()
                {
                    CustomerGroupName = "Tổng",
                    QtyECCB = sumQtyEccB,
                    QtyCRMB = sumQtyCRMB,
                    QtyTotalB = sumQtyTotalB,
                    QtyECCC = sumQtyEccC,
                    QtyCRMC = sumQtyCRMC,
                    QtyTotalC = sumQtyTotalC,
                    QtyECC = sumQtyEcc,
                    QtyCRM = sumQtyCRM,
                    QtyTotal = sumQtyTotal,
                };

                CustomerAmountByCRMViewModel totalPercent = new CustomerAmountByCRMViewModel()
                {
                    CustomerGroupName = "Tỷ lệ (%)",
                    QtyECCB = sumQtyTotalB != 0 ? (sumQtyEccB * 100) / sumQtyTotalB : 0,
                    QtyCRMB = sumQtyTotalB != 0 ? (sumQtyCRMB * 100) / sumQtyTotalB : 0,
                    QtyTotalB = sumQtyTotalB != 0 ? (sumQtyTotalB * 100) / sumQtyTotalB : 0,
                    QtyECCC = sumQtyTotalC != 0 ? (sumQtyEccC * 100) / sumQtyTotalC : 0,
                    QtyCRMC = sumQtyTotalC != 0 ? (sumQtyCRMC * 100) / sumQtyTotalC : 0,
                    QtyTotalC = sumQtyTotalC != 0 ? (sumQtyTotalC * 100) / sumQtyTotalC : 0,
                    QtyECC = sumQtyTotal != 0 ? (sumQtyEcc * 100) / sumQtyTotal : 0,
                    QtyCRM = sumQtyTotal != 0 ? (sumQtyCRM * 100) / sumQtyTotal : 0,
                    QtyTotal = sumQtyTotal != 0 ? (sumQtyTotal * 100) / sumQtyTotal : 0,
                };

                viewModel.Add(total);
                viewModel.Add(totalPercent);
            }
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            //columns.Add(new ExcelTemplate { ColumnName = "CustomerTypeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CustomerGroupName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyECCB", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth=13, AnotherName = "Khách ECC", MergeHeaderTitle = "Doanh nghiệp" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyCRMB", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Khách CRM", MergeHeaderTitle = "Doanh nghiệp" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyTotalB", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Tổng", MergeHeaderTitle = "Doanh nghiệp" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyECCC", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Khách ECC", MergeHeaderTitle = "Tiêu dùng" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyCRMC", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Khách CRM", MergeHeaderTitle = "Tiêu dùng" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyTotalC", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Tổng", MergeHeaderTitle = "Tiêu dùng" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyECC", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Khách ECC", MergeHeaderTitle = "Tổng" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyCRM", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Khách CRM", MergeHeaderTitle = "Tổng" });
            columns.Add(new ExcelTemplate { ColumnName = "QtyTotal", isAllowedToEdit = false, hasAnotherName = true, isNumber = true, CustomWidth = 13, AnotherName = "Tổng", MergeHeaderTitle = "Tổng" });
            #endregion Master
         

            //Header
            string fileheader = "BÁO CÁO TỶ LỆ KHÁCH HÀNG ECC/CRM";
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
            //if (!string.IsNullOrEmpty(CommonDate) && CommonDate != "Custom")
            //{
            //    var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate).ToList();
            //    var commonDateStr = commonDateList.Where(p => p.CatalogCode == CommonDate).Select(p => p.CatalogText_vi).FirstOrDefault();
            //    heading.Add(new ExcelHeadingTemplate()
            //    {
            //        Content = string.Format("Tỷ lệ: {0} ({1:dd/MM/yyyy}-{2:dd/MM/yyyy} so với {3:dd/MM/yyyy}-{4:dd/MM/yyyy})", commonDateStr, excelFromDate, excelToDate, excelRatioFromDate, excelRatioToDate),
            //        RowsToIgnore = 1,
            //        isWarning = false,
            //        isCode = true
            //    });
            //}

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, headerRowMergeCount: 1);

            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        private List<CustomerAmountByCRMViewModel> GetData(CustomerAmountByCRMSearchViewModel searchModel)
        {
            string sqlQuery = "EXEC Report.usp_CustomerAmountByCRMReport @CustomerTypeCode, @CustomerGroupCode, @CurrentCompanyCode, @FromDate, @ToDate";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerTypeCode",
                    Value = searchModel.CustomerTypeCode ?? (object)DBNull.Value
                },new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerGroupCode",
                    Value = searchModel.CustomerGroupCode ?? (object)DBNull.Value
                },new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = searchModel.CompanyCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                }
            };

            var data = _context.Database.SqlQuery<CustomerAmountByCRMViewModel>(sqlQuery, parameters.ToArray()).ToList();
            
            return data;
        }
        public ActionResult ViewDetail(CustomerAmountByCRMSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, CustomerAmountByCRMSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerAmountByCRMModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CustomerAmountByCRMGridPartial(Guid? templateId = null, CustomerAmountByCRMSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CustomerAmountByCRM");
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
            CreateViewBag();
            if ((string.IsNullOrEmpty(jsonReq) || jsonReq == "null") && (searchViewModel == null || searchViewModel.IsView != true))
            {
                ViewBag.Search = null;
                return PartialView("_CustomerAmountByCRMPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<CustomerAmountByCRMSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerAmountByCRMPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(CustomerAmountByCRMSearchViewModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_SO_LUONG_KHACH_THEO_CRM_ECC";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(CustomerAmountByCRMSearchViewModel searchViewModel)
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
            if (!string.IsNullOrEmpty(searchViewModel.CompanyCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công Ty";
                var company = _context.CompanyModel.FirstOrDefault(s => s.CompanyCode == searchViewModel.CompanyCode);
                var value = company.CompanyName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.CustomerGroupCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm khách hàng";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.CustomerGroupCode && s.CatalogTypeCode == ConstCatalogType.CustomerGroup).CatalogText_vi;
                filter.DisplayValue = value;
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

            return filterList;
        }
        #endregion


        #region Helper

        private void CreateViewBag()
        {
            var customerTypeList = _context.CatalogModel.Where(
               p => p.CatalogTypeCode == ConstCatalogType.CustomerCategory
               && p.CatalogCode != ConstCustomerType.Contact
               && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.customerTypeList = customerTypeList;
            ViewBag.CustomerTypeCode = new SelectList(customerTypeList, "CatalogCode", "CatalogText_vi");

            var _catalogRepository = new CatalogRepository(_context);
            var customerGroupList = _catalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi");

            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName", CurrentUser.CompanyCode);
        }
        #endregion
    }
}