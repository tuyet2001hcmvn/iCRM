using DevExpress.Web.Internal;
using DevExpress.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ShowroomReportController : BaseController
    {
        // GET: ShowroomReport
        #region Index
        public ActionResult Index()
        {
            var searchModel = (ShowroomReportSearchViewModel)TempData[CurrentUser.AccountId + "ShowroomSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ShowroomTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ShowroomModeSearch"];
            var pageId = GetPageId("/Reports/ShowroomReport");

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

        public void CreateViewBag()
        {
            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion

            #region //Company (Công ty)
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");
            #endregion

            #region //Get list Sale Office (Khu vực)
            var saleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.Area = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //TaskStatusCode (Trạng thái)
            var result = (from wf in _context.WorkFlowModel
                          join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                          where wf.WorkflowCategoryCode == ConstWorkFlowCategory.GTB
                          select new
                          {
                              ts.OrderIndex,
                              ts.TaskStatusCode,
                              ts.TaskStatusName
                          }).Distinct().OrderBy(p => p.OrderIndex).ToList();
            ViewBag.TaskStatusCode = new SelectList(result, "TaskStatusCode", "TaskStatusName");
            #endregion

            #region //WorkFlowId
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.GTB);
            ViewBag.WorkFlowId = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName");
            #endregion

            #region Loại catalogue (Nhóm VT)
            var catalogCategoryList = _context.View_Catalog_Category.OrderBy(p => p.OrderIndex).Select(p => new ISDSelectGuidItem()
            {
                id = p.CategoryId,
                name = p.CategoryName,
            }).ToList();

            ViewBag.CategoryId = new SelectList(catalogCategoryList, "id", "name");
            #endregion

        }
        #endregion

        #region Export Excel
        public ActionResult ExportExcel(ShowroomReportSearchViewModel searchModel)
        {
            List<ShowroomReportViewModel> result = new List<ShowroomReportViewModel>();
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            result = GetData(searchModel);
            return Export(result, filterDisplayList);
        }
        public ActionResult Export(List<ShowroomReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO TỔNG HỢP ĐIỂM TRƯNG BÀY";
            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaskStatusName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Area", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "NumberOfShowroom", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ValueOfShowroom", isAllowedToEdit = false, isCurrency = true });

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

        public ActionResult ViewDetail(ShowroomReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ShowroomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ShowroomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ShowroomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ShowroomReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ShowroomSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ShowroomTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ShowroomModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ShowroomPivotGridPartial(Guid? templateId = null, ShowroomReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ShowroomReport");
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
                return PartialView("_ShowroomPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ShowroomReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ShowroomPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ShowroomReportSearchViewModel searchViewModel, Guid? templateId)
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
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            var headerText = string.Empty;

            if (template != null)
            {
                headerText = template.TemplateName;
            }
            string fileName = "BAO_CAO_TONG_HOP_DIEM_TRUNG_BAY";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        private List<ShowroomReportViewModel> GetData(ShowroomReportSearchViewModel searchModel)
        {
            List<ShowroomReportViewModel> result = new List<ShowroomReportViewModel>();

            #region WorkFlowId
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (searchModel.WorkFlowId != null && searchModel.WorkFlowId.Count > 0)
            {
                foreach (var r in searchModel.WorkFlowId)
                {
                    var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWorkFlow.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                tableWorkFlow.Add(tableRow);
            }
            #endregion

            #region TaskStatusCode
            //Build your record
            var tableStatusSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableStatus = new List<SqlDataRecord>();
            if (searchModel.TaskStatusCode != null && searchModel.TaskStatusCode.Count > 0)
            {
                foreach (var r in searchModel.TaskStatusCode)
                {
                    var tableRow = new SqlDataRecord(tableStatusSchema);
                    tableRow.SetString(0, r);
                    tableStatus.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableStatusSchema);
                tableStatus.Add(tableRow);
            }
            #endregion

            #region CategoryId
            //Build your record
            var tableCategorySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableCategory = new List<SqlDataRecord>();
            if (searchModel.CategoryId != null && searchModel.CategoryId.Count > 0)
            {
                foreach (var r in searchModel.CategoryId)
                {
                    var tableRow = new SqlDataRecord(tableCategorySchema);
                    tableRow.SetSqlGuid(0, r);
                    tableCategory.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCategorySchema);
                tableCategory.Add(tableRow);
            }
            #endregion

            #region Khu vực
            //Build your record
            var tableAreaSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableArea = new List<SqlDataRecord>();
            if (searchModel.Area != null && searchModel.Area.Count > 0)
            {
                foreach (var r in searchModel.Area)
                {
                    var tableRow = new SqlDataRecord(tableAreaSchema);
                    tableRow.SetString(0, r);
                    tableArea.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableAreaSchema);
                tableArea.Add(tableRow);
            }
            #endregion

            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_ShowroomReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        var table = sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", tableWorkFlow);
                        table.SqlDbType = SqlDbType.Structured;
                        table.TypeName = "[dbo].[GuidList]";
                        var table2 = sda.SelectCommand.Parameters.AddWithValue("@TaskStatusCode", tableStatus);
                        table2.SqlDbType = SqlDbType.Structured;
                        table2.TypeName = "[dbo].[StringList]";
                        var table3 = sda.SelectCommand.Parameters.AddWithValue("@Area", tableArea);
                        table3.SqlDbType = SqlDbType.Structured;
                        table3.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@CompanyId", searchModel.CompanyId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StartFromDate", searchModel.StartFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StartToDate", searchModel.StartToDate ?? (object)DBNull.Value);

                        var table4 = sda.SelectCommand.Parameters.AddWithValue("@CategoryId", tableCategory);
                        table4.SqlDbType = SqlDbType.Structured;
                        table4.TypeName = "[dbo].[GuidList]";
                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                #region Convert to list
                                ShowroomReportViewModel model = new ShowroomReportViewModel();
                                model.WorkFlowName = item["WorkFlowName"].ToString();
                                model.TaskStatusName = item["TaskStatusName"].ToString();
                                model.Area = item["Area"].ToString();
                                if (!string.IsNullOrEmpty(item["NumberOfShowroom"].ToString()))
                                {
                                    model.NumberOfShowroom = Convert.ToDecimal(item["NumberOfShowroom"].ToString());
                                }
                                if (!string.IsNullOrEmpty(item["ValueOfShowroom"].ToString()))
                                {
                                    model.ValueOfShowroom = Convert.ToDecimal(item["ValueOfShowroom"].ToString());
                                }
                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }
            return result;
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ShowroomReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId.Count > 0 && searchViewModel.WorkFlowId[0] != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Loại";
                var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.GTB);
                foreach (var wf in searchViewModel.WorkFlowId)
                {
                    var workFlow = listWorkFlow.FirstOrDefault(s => s.WorkFlowId == wf);
                    filter.DisplayValue += workFlow.WorkFlowName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }

            if (searchViewModel.TaskStatusCode != null && searchViewModel.TaskStatusCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trạng thái";
                var result = (from wf in _context.WorkFlowModel
                              join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                              where wf.WorkflowCategoryCode == ConstWorkFlowCategory.GTB
                              select new
                              {
                                  ts.OrderIndex,
                                  ts.TaskStatusCode,
                                  ts.TaskStatusName
                              }).Distinct().OrderBy(p => p.OrderIndex).ToList();
                foreach (var code in searchViewModel.TaskStatusCode)
                {
                    var workFlow = result.FirstOrDefault(s => s.TaskStatusCode == code);
                    filter.DisplayValue += workFlow.TaskStatusName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }

            if (searchViewModel.CompanyId != null && searchViewModel.CompanyId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công ty";
                var value = _context.CompanyModel.FirstOrDefault(s => s.CompanyId == searchViewModel.CompanyId).CompanyName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }

            if (searchViewModel.Area != null && searchViewModel.Area.Count > 0)
            {
                var saleOffice = _context.CatalogModel.Where(s => searchViewModel.Area.Contains(s.CatalogCode) && s.CatalogTypeCode == ConstCatalogType.SaleOffice).Select(p => p.CatalogText_vi).ToList();
                if (saleOffice != null && saleOffice.Count() > 0)
                {
                    SearchDisplayModel filter = new SearchDisplayModel();
                    filter.DisplayName = "Khu vực";
                    var value = string.Join(", ", saleOffice);
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }

            }

            if (searchViewModel.CategoryId != null && searchViewModel.CategoryId.Count > 0 && searchViewModel.CategoryId[0] != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm vật tư";
                foreach (var id in searchViewModel.CategoryId)
                {
                    var category = _context.View_Catalog_Category.FirstOrDefault(s => s.CategoryId == id);
                    filter.DisplayValue += category.CategoryName;
                    filter.DisplayValue += ", ";
                }
               
                filterList.Add(filter);
            }
            if (searchViewModel.StartFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy") });
            }
            return filterList;
        }
        #endregion

        // Post : Print PDF
        #region In báo cáo
        [HttpPost]
        public ActionResult Print(ShowroomReportSearchViewModel searchViewModel, Guid? templateId)
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