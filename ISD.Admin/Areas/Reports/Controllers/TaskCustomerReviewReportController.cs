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
    public class TaskCustomerReviewReportController : BaseController
    {
        // GET: TaskCustomerReviewReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            #region ViewBag
            #region //Get list ServiceTechnicalTeamCode (Trung tâm bảo hành)
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");
            #endregion
            //Employee
            var empLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            //Assignee
            ViewBag.AssigneeList = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC);
            listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();

            ViewBag.WorkFlowIdList = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName");
            //Ý kiến khách hàng
            var customerRatings = new List<SelectListItem>();
            customerRatings.Add(new SelectListItem()
            {
                Value = "none",
                Text = "Không ý kiến"
            });
            customerRatings.Add(new SelectListItem()
            {
                Value = "rating",
                Text = "Đánh giá theo sao & ý kiến khác"
            });
            ViewBag.Property5 = new SelectList(customerRatings, "Value", "Text");
            #region //Get list Roles (Phòng ban)
            //var rolesList = _context.DepartmentModel.Where(p => p.Actived == true).ToList();
            //ViewBag.DepartmentCode = new SelectList(rolesList, "DepartmentCode", "DepartmentName");
            var TaskRolesList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true).Where(p => p.RolesCode.Contains("2000")).ToList();
            ViewBag.DepartmentCode = new SelectList(TaskRolesList, "RolesCode", "RolesName");
            #endregion
            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            //Common Date 2
            var commonDateList2 = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate2 = new SelectList(commonDateList2, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion
            #endregion
            var searchModel = (TaskTicketMLCReportSearchModel)TempData[CurrentUser.AccountId + "TaskCustomerReviewSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "TaskCustomerReviewTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "TaskCustomerReviewModeSearch"];
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
            var pageId = GetPageId("/Reports/TaskCustomerReviewReport");
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
            return View();
        }

        public ActionResult ExportExcel(TaskTicketMLCReportSearchModel searchModel)
        {
            var data = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(data, filterDisplayList);
        }

        private FileContentResult Export(List<TaskTicketMLCReportExcelModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Column
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "TaskAssigneeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ServiceRating", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductRating", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Property5", isAllowedToEdit = false });

            #endregion
            //Header
            string fileheader = "Báo cáo Tổng hợp ý kiến khách hàng";

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

        private List<TaskTicketMLCReportExcelModel> GetData(TaskTicketMLCReportSearchModel searchModel)
        {
            #region //End Date
            if (searchModel.EndCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchModel.EndCommonDate, out fromDate, out toDate);
                searchModel.EndFromDate = fromDate;
                searchModel.EndToDate = toDate;
            }
            #endregion

            #region WorkflowList
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
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

            var result = new List<TaskTicketMLCReportExcelModel>();
            //int FilteredResultsCount = 0;
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_TaskCustomerReviewReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@EndFromDate", searchModel.EndFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EndToDate", searchModel.EndToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ProfileId", searchModel.ProfileId ?? (object)DBNull.Value);
                        //sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", searchModel.WorkFlowId ?? (object)DBNull.Value);
                        var tablework = sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", tableWorkFlow);
                        tablework.SqlDbType = SqlDbType.Structured;
                        tablework.TypeName = "[dbo].[GuidList]";
                        //sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", searchModel.ServiceTechnicalTeamCode ?? (object)DBNull.Value);
                        var ServiceTechnicalTeamCode = sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", tableServiceTechnicalTeamCode);
                        ServiceTechnicalTeamCode.SqlDbType = SqlDbType.Structured;
                        ServiceTechnicalTeamCode.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@Assignee", searchModel.Assignee ?? (object)DBNull.Value);
                        //sda.SelectCommand.Parameters.AddWithValue("@DepartmentCode", searchModel.DepartmentCode ?? (object)DBNull.Value);
                        var DepartmentCode = sda.SelectCommand.Parameters.AddWithValue("@DepartmentCode", tableDepartmentCode);
                        DepartmentCode.SqlDbType = SqlDbType.Structured;
                        DepartmentCode.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@Property5", searchModel.Property5 ?? (object)DBNull.Value);
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
                                var model = new TaskTicketMLCReportExcelModel();
                                model.WorkFlowName = item["WorkFlowName"].ToString();
                                model.TaskAssigneeName = item["TaskAssigneeName"].ToString();
                                model.ServiceRating = item["ServiceRating"].ToString();
                                model.ProductRating = item["ProductRating"].ToString();
                                model.Property5 = item["Property5"].ToString();
                                result.Add(model);
                                #endregion
                            }
                        }
                    }
                }
            }
            return result;
        }
        public ActionResult ViewDetail(TaskTicketMLCReportSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskCustomerReviewSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskCustomerReviewTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskCustomerReviewModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TaskTicketMLCReportSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskCustomerReviewSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskCustomerReviewTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskCustomerReviewModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult TaskCustomerReviewPivotGridPartial(Guid? templateId = null, TaskTicketMLCReportSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TaskCustomerReviewReport");
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
                return PartialView("_TaskCustomerReviewPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TaskTicketMLCReportSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_TaskCustomerReviewPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(TaskTicketMLCReportSearchModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_TONG_HOP_Y_KIEN_KHACH_HANG";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TaskTicketMLCReportSearchModel searchViewModel)
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
            if (searchViewModel.ServiceTechnicalTeamCode != null && searchViewModel.ServiceTechnicalTeamCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trung tâm bảo hành";
                var listCatalog = _context.CatalogModel.Where(s => s.CatalogTypeCode == ConstCatalogType.ServiceTechnicalTeam).ToList();
                var value = listCatalog.Where(x => searchViewModel.ServiceTechnicalTeamCode.Contains(x.CatalogCode)).Select(x => x.CatalogText_vi).ToList();
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (searchViewModel.ProfileId != null && searchViewModel.ProfileId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khách hàng";
                var value = _context.ProfileModel.FirstOrDefault(s => s.ProfileId == searchViewModel.ProfileId).ProfileName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
           
            if (!string.IsNullOrEmpty(searchViewModel.Assignee))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên được phân công";
                var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == searchViewModel.Assignee);
                var value = sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            //if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId != Guid.Empty)
            //{
            //    SearchDisplayModel filter = new SearchDisplayModel();
            //    filter.DisplayName = "Loại";
            //    var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC);
            //    listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
            //    var value = listWorkFlow.FirstOrDefault(s => s.WorkFlowId == searchViewModel.WorkFlowId).WorkFlowName;
            //    filter.DisplayValue = value;
            //    filterList.Add(filter);
            //}
            if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Loại";
                var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC);
                listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
                var value = listWorkFlow.Where(p => searchViewModel.WorkFlowId.Contains(p.WorkFlowId)).Select(p => p.WorkFlowName).ToList();
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (searchViewModel.DepartmentCode != null && searchViewModel.DepartmentCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                //var dep = _context.DepartmentModel.FirstOrDefault(s => s.DepartmentId == searchViewModel.DepartmentId);
                var value = _context.RolesModel.Where(s => searchViewModel.DepartmentCode.Contains(s.RolesCode)).Select(x => x.RolesName).ToList();
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }

            if (!string.IsNullOrEmpty(searchViewModel.Property5))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Ý kiến khách hàng";
                if (searchViewModel.Property5 == "none")
                {
                    filter.DisplayValue = "Không ý kiến";
                }
                else
                {
                    filter.DisplayValue = "Đánh giá theo sao & ý kiến khác";
                }

                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}