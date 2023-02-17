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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class MaterialExportedDetailReportController : BaseController
    {
        // GET: MaterialExportedDetailReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            MaterialExportedDetailReportSearchViewModel searchViewModel = new MaterialExportedDetailReportSearchViewModel()
            {
                //Ngày ghé thăm
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate,
            };
            var searchModel = (MaterialExportedDetailReportSearchViewModel)TempData[CurrentUser.AccountId + "MaterialExportedDetailSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "MaterialExportedDetailTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "MaterialExportedDetailModeSearch"];
            var pageId = GetPageId("/Reports/MaterialExportedDetailReport");
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
            if (searchModel != null)
            {
                searchViewModel = searchModel;
            }
            CreateSearchViewBag(searchViewModel);
            return View(searchViewModel);
        }
        #endregion Index

        public ActionResult ViewDetail(MaterialExportedDetailReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "MaterialExportedDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "MaterialExportedDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "MaterialExportedDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, MaterialExportedDetailReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "MaterialExportedDetailSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "MaterialExportedDetailTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "MaterialExportedDetailModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        private string GetDepartmentName(Guid? ProfileId)
        {
            var departmentName = string.Empty;
            //Nhân viên kinh doanh
            var saleSupervisorList = (from p in _context.PersonInChargeModel
                                      join acc in _context.AccountModel on p.SalesEmployeeCode equals acc.EmployeeCode
                                      from ro in acc.RolesModel
                                      where p.ProfileId == ProfileId && p.CompanyCode == CurrentUser.CompanyCode
                                      select new
                                      {
                                          SalesEmployeeCode = p.SalesEmployeeCode,
                                          DepartmentName = ro.RolesName,
                                          isEmployeeGroup = ro.isEmployeeGroup
                                      }).ToList();
            //Lấy 1 thông tin phòng ban NV kinh doanh
            if (saleSupervisorList != null && saleSupervisorList.Count > 0)
            {
                var SaleSupervisor = saleSupervisorList.Where(p => p.isEmployeeGroup == true).FirstOrDefault();
                if (SaleSupervisor == null)
                {
                    SaleSupervisor = saleSupervisorList.FirstOrDefault();
                }
                departmentName = SaleSupervisor.isEmployeeGroup == true ? SaleSupervisor.DepartmentName : "";
            }

            return departmentName;
        }

        #region CreateSearchViewBag
        private void CreateSearchViewBag(MaterialExportedDetailReportSearchViewModel searchViewModel)
        {
            var listWorkFlow = new List<WorkFlowViewModel>();
            listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.GTB);
            listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
            ViewBag.WorkFlowId = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);

            //Khu vực
            var saleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");

            //Phòng ban
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new MultiSelectList(rolesList, "RolesCode", "RolesName");
        }   
        [ValidateInput(false)]
        public ActionResult MaterialExportedDetailPivotGridPartial(Guid? templateId = null, MaterialExportedDetailReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/MaterialExportedDetailReportController");
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
                return PartialView("_MaterialExportedDetailPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<MaterialExportedDetailReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_MaterialExportedDetailPivotGridPartial", model);
            }
        }
        private List<MaterialExportedDetailReportViewModel> GetData(MaterialExportedDetailReportSearchViewModel searchModel)
        {
            if (searchModel.CommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;

                _unitOfWork.CommonDateRepository.GetDateBy(searchModel.CommonDate, out fromDate, out toDate);
                searchModel.FromDate = fromDate;
                searchModel.ToDate = toDate;
            }
            #region Loại ĐTB
            //Build your record
            var tableWorkFlowIdSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableWorkFlowId = new List<SqlDataRecord>();
            List<Guid> workFlowIdLst = new List<Guid>();
            if (searchModel.WorkFlowId == null)
            {
                searchModel.WorkFlowId = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.GTB).Where(p => p.WorkFlowCode != ConstWorkFlow.GT).Select(x => x.WorkFlowId).ToList();
            }
            foreach (var r in searchModel.WorkFlowId)
            {
                var tableRow = new SqlDataRecord(tableWorkFlowIdSchema);
                tableRow.SetGuid(0, r);
                if (!workFlowIdLst.Contains(r))
                {
                    workFlowIdLst.Add(r);
                    tableWorkFlowId.Add(tableRow);
                }
            }
            #endregion

            #region Phòng ban
            //Build your record
            var tableRolesCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableRolesCode = new List<SqlDataRecord>();
            List<string> rolesCodeLst = new List<string>();
            if (searchModel.RolesCode != null && searchModel.RolesCode.Count > 0)
            {
                foreach (var r in searchModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                    tableRow.SetString(0, r);
                    if (!rolesCodeLst.Contains(r))
                    {
                        rolesCodeLst.Add(r);
                        tableRolesCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion

            #region Khu vực
            //Build your record
            var tableSaleOfficeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSaleOfficeCode = new List<SqlDataRecord>();
            List<string> saleOfficeCodeLst = new List<string>();
            if (searchModel.SaleOfficeCode != null && searchModel.SaleOfficeCode.Count > 0)
            {
                foreach (var r in searchModel.SaleOfficeCode)
                {
                    var tableRow = new SqlDataRecord(tableSaleOfficeCodeSchema);
                    tableRow.SetString(0, r);
                    if (!saleOfficeCodeLst.Contains(r))
                    {
                        saleOfficeCodeLst.Add(r);
                        tableSaleOfficeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSaleOfficeCodeSchema);
                tableSaleOfficeCode.Add(tableRow);
            }
            #endregion



            string sqlQuery = "EXEC [Report].[usp_MaterialExportDetailGVL] @WorkFlowId, @SaleOfficeCode, @RolesCode, @FromDate, @ToDate";

            List<SqlParameter> parameters = new List<SqlParameter>
            {

                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowId",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableWorkFlowId

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOfficeCode",
                    TypeName = "[dbo].[StringList]",
                    Value = tableSaleOfficeCode

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]",
                    Value = tableRolesCode
                    //Value = searchModel.RolesCode ?? (object)DBNull.Value,
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

            var data = _context.Database.SqlQuery<MaterialExportedDetailReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            //if (data != null && data.Count > 0)
            //{
            //    foreach (var item in data)
            //    {

            //        item.DepartmentName = GetDepartmentName(item.ProfileId);
            //        if (!string.IsNullOrEmpty(item.SaleOfficeCode))
            //        {
            //            item.AreaName = _context.CatalogModel.FirstOrDefault(p => p.CatalogCode == item.SaleOfficeCode && p.CatalogTypeCode == ConstCatalogType.SaleOffice).CatalogText_vi;
            //        }
            //    }
            //}

            //var newData = (from p in data
            //               group p by new { p.AreaName, p.DepartmentName } into gr
            //               select new MaterialExportedDetailReportViewModel
            //               {
            //                   AreaName = gr.Key.AreaName,
            //                   DepartmentName = gr.Key.DepartmentName,
            //                   QtyKeCTL = gr.Sum(p => p.QtyKeCTL),
            //                   QtyKeMau = gr.Sum(p => p.QtyKeMau),
            //                   QtyBasInox = gr.Sum(p => p.QtyBasInox),
            //                   QtyBangHieu = gr.Sum(p => p.QtyBangHieu),
            //                   QtyKhayA5 = gr.Sum(p => p.QtyKhayA5)
            //               }).OrderBy(p => p.AreaName).ToList();
            return data;
        }






        #endregion

        #region Export to Excel
        public ActionResult ExportExcel(MaterialExportedDetailReportSearchViewModel searchViewModel)
        {

            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var data = GetData(searchViewModel);

            return Export(data, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<MaterialExportedDetailReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "RolesName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Quantity", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyKeCTL", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyKeMau", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyBasInox", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyBangHieu", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyKhayA5", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyMauA5", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "QtyMau250x370", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = LanguageResource.Report_MaterialExportedDetail;

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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);

            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Excel

        #region Export Pivot

        [HttpPost]
        public ActionResult ExportPivot(MaterialExportedDetailReportSearchViewModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_CT_XUAT_VAT_TU_CHO_DTB";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #endregion

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(MaterialExportedDetailReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.SaleOfficeCode != null && searchViewModel.SaleOfficeCode.Count > 0)
            {
                var saleOffice = _context.CatalogModel.Where(s => searchViewModel.SaleOfficeCode.Contains(s.CatalogCode) && s.CatalogTypeCode == ConstCatalogType.SaleOffice).Select(p => p.CatalogText_vi).ToList();
                if (saleOffice != null && saleOffice.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Khu vực";
                    var value = string.Join(", ", saleOffice);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
              
            }
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {

                var roles = _context.RolesModel.Where(s => searchViewModel.RolesCode.Contains(s.RolesCode)).Select(p => p.RolesName).ToList();
                if (roles != null && roles.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Phòng ban";
                    var value = string.Join(", ", roles);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId.Count > 0)
            {

                var workFlow = _context.WorkFlowModel.Where(s => searchViewModel.WorkFlowId.Contains(s.WorkFlowId)).Select(p => p.WorkFlowName).ToList();
                if (workFlow != null && workFlow.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Loại ĐTB";
                    var value = string.Join(", ", workFlow);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
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

        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print(MaterialExportedDetailReportSearchViewModel searchViewModel, Guid? templateId)
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