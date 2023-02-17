using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class TaskGroupReportController : BaseController
    {
        #region Index
        // GET: TaskGroupReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            
            var searchModel = (TaskGroupReportSerchViewModel)TempData[CurrentUser.AccountId + "TaskGroupSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "TaskGroupTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "TaskGroupModeSearch"];
            var pageId = GetPageId("/Reports/TaskGroupReport");
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
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            TaskGroupReportSerchViewModel searchViewModel = new TaskGroupReportSerchViewModel()
            {
                //Ngày ghé thăm
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate,
            };
            if (searchModel != null)
            {
                searchViewModel = searchModel;
            }
            CreateSearchViewBag(searchViewModel);
            return View(searchViewModel);
        }


        #endregion Index

        #region CreateViewBag
        private void CreateSearchViewBag( TaskGroupReportSerchViewModel searchViewModel)
        {
            //ViewBag.Type = Type;

            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            //Common Date 2
            var commonDateList2 = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate2 = new SelectList(commonDateList2, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion



            //Assignee
            var empLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.Assignee = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");
            //Reporter
            ViewBag.Reporter = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");
            ////RolesCode
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");

            //CreateByName
            var accounts = _unitOfWork.AccountRepository.GetAll();
            //CreateBy
            ViewBag.CreateBy = new SelectList(accounts, "EmployeeCode", "UserName");

            //TaskProcessCode
            var statusLst = _unitOfWork.TaskStatusRepository.GetTaskStatusList();
            ViewBag.TaskProcessCode = new SelectList(statusLst, "StatusCode", "StatusName");

            #region //TaskStatusCode (Trạng thái)
            var isShowTaskStatusCode = true;
            var result = (from wf in _context.WorkFlowModel
                          join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                          where wf.WorkflowCategoryCode == ConstWorkFlowCategory.MISSION
                          select new
                          {
                              ts.OrderIndex,
                              ts.TaskStatusCode,
                              ts.TaskStatusName
                          }).Distinct().OrderBy(p => p.OrderIndex).ToList();
            if (result != null && result.Count > 0)
            {
                ViewBag.TaskStatusCode = new SelectList(result, "TaskStatusCode", "TaskStatusName");
            }
            else
            {
                isShowTaskStatusCode = false;
            }
            ViewBag.isShowTaskStatusCode = isShowTaskStatusCode;
            #endregion

          

        }

        #endregion

        #region Export Pivot
        [HttpPost]
        public ActionResult ExportPivot(TaskGroupReportSerchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            //var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            //get pivot config 
            //Lấy cột - thứ tự cột ... từ bảng SearchResultDetailTemplateModel
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            //Lấy thông tin config các thông số người dùng lưu template từ SearchResultTemplateModel.LayoutConfigs
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];

            try
            {
                string fileName = (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs, PagerSize: ePaperSize.A4, Scale: 70, Orientation: eOrientation.Landscape);

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TaskGroupReportSerchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.CreatedFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo từ ngày", DisplayValue = searchViewModel.CreatedFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CreatedToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo đến ngày", DisplayValue = searchViewModel.CreatedToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Bắt đầu từ ngày", DisplayValue = searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Bắt đầu đến ngày", DisplayValue = searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (!string.IsNullOrEmpty(searchViewModel.TaskProcessCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm trạng thái";
                var statusLst = _unitOfWork.TaskStatusRepository.GetTaskStatusList();
                var value = statusLst.FirstOrDefault(s => s.StatusCode == searchViewModel.TaskProcessCode).StatusName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            
            if (searchViewModel.Assignee != null && searchViewModel.Assignee.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên được phân công";
                foreach (var code in searchViewModel.Assignee)
                {
                    var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == code);
                    var value = sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                    filter.DisplayValue += value;
                    filter.DisplayValue += ", ";
                }

                filterList.Add(filter);
            }

            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                foreach (var id in searchViewModel.RolesCode)
                {
                    filter.DisplayValue += _context.RolesModel.FirstOrDefault(s => s.RolesCode == id).RolesName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            
           
            return filterList;
        }
         #endregion

        public ActionResult ViewDetail(TaskGroupReportSerchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskGroupSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskGroupTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskGroupModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TaskGroupReportSerchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskGroupSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskGroupTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskGroupModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult TaskGroupReportPivotGridPartial(Guid? templateId = null, TaskGroupReportSerchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TaskGroupReport");
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
                    if (searchViewModel == null)
                    {
                        searchViewModel = new TaskGroupReportSerchViewModel();
                    }
                    searchViewModel.OrderBy = Template.OrderBy;
                    searchViewModel.TypeSort = Template.TypeSort;
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
                return PartialView("_TaskGroupReportPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TaskGroupReportSerchViewModel>(jsonReq);
                }

                if (templateId.HasValue)
                {
                    var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
                    if (pivotTemplate != null)
                    {
                        searchViewModel.ReportType = pivotTemplate.TemplateName;
                    }
                }


                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_TaskGroupReportPivotGridPartial", model);
            }
        }

        #region GetData
        private List<TaskGroupReportViewModel> GetData(TaskGroupReportSerchViewModel searchViewModel)
        {
            searchViewModel.CompanyId = searchViewModel.CompanyId;
            searchViewModel.CompanyName = searchViewModel.CompanyName;

            List<string> processCodeList = new List<string>();
            if (searchViewModel.TaskProcessCode == null)
            {
                processCodeList.Add(ConstTaskStatus.Todo);
                processCodeList.Add(ConstTaskStatus.Processing);
                processCodeList.Add(ConstTaskStatus.Incomplete);
                processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                processCodeList.Add(ConstTaskStatus.CompletedExpire);
                processCodeList.Add(ConstTaskStatus.Expired);
            }


            #region //Start Date
            if (searchViewModel.StartCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.StartCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.StartFromDate = fromDate;
                searchViewModel.StartToDate = toDate;
            }
            #endregion

            #region //End Date
            if (searchViewModel.EndCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.EndCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.EndFromDate = fromDate;
                searchViewModel.EndToDate = toDate;
            }
            #endregion

            #region Task Process
            var TaskProcessCode_Todo = false;
            var TaskProcessCode_Processing = false;
            var TaskProcessCode_Incomplete = false;
            var TaskProcessCode_CompletedOnTime = false;
            var TaskProcessCode_CompletedExpire = false;
            var TaskProcessCode_Expired = false;

            if (processCodeList != null && processCodeList.Count > 0)
            {
                foreach (var item in processCodeList)
                {
                    //Việc cần làm
                    if (item == ConstTaskStatus.Todo)
                    {
                        TaskProcessCode_Todo = true;
                    }
                    //Đang thực hiện
                    if (item == ConstTaskStatus.Processing)
                    {
                        TaskProcessCode_Processing = true;
                    }
                    //Chưa hoàn thành
                    if (item == ConstTaskStatus.Incomplete)
                    {
                        TaskProcessCode_Incomplete = true;
                    }
                    //Hoàn thành đúng hạn
                    if (item == ConstTaskStatus.CompletedOnTime)
                    {
                        TaskProcessCode_CompletedOnTime = true;
                    }
                    //Hoàn thành quá hạn
                    if (item == ConstTaskStatus.CompletedExpire)
                    {
                        TaskProcessCode_CompletedExpire = true;
                    }
                    //Quá hạn
                    if (item == ConstTaskStatus.Expired)
                    {
                        TaskProcessCode_Expired = true;
                    }
                }
            }
            else
            {
                //Việc cần làm
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.Todo)
                {
                    TaskProcessCode_Todo = true;
                }
                //Đang thực hiện
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.Processing)
                {
                    TaskProcessCode_Processing = true;
                }
                //Chưa hoàn thành
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.Incomplete)
                {
                    TaskProcessCode_Incomplete = true;
                }
                //Hoàn thành đúng hạn
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime)
                {
                    TaskProcessCode_CompletedOnTime = true;
                }
                //Hoàn thành quá hạn
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.CompletedExpire)
                {
                    TaskProcessCode_CompletedExpire = true;
                }
                //Quá hạn
                if (searchViewModel.TaskProcessCode == ConstTaskStatus.Expired)
                {
                    TaskProcessCode_Expired = true;
                }
            }
            #endregion

            #region TaskStatusCodeList - Trạng thái
            //Build your record
            var tableTaskStatusCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableTaskStatusCode = new List<SqlDataRecord>();
            if (searchViewModel.TaskStatusCode != null && searchViewModel.TaskStatusCode.Count > 0)
            {
                foreach (var t in searchViewModel.TaskStatusCode)
                {
                    var tableRow = new SqlDataRecord(tableTaskStatusCodeSchema);
                    tableRow.SetString(0, t);
                    tableTaskStatusCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableTaskStatusCodeSchema);
                tableTaskStatusCode.Add(tableRow);
            }
            #endregion

            #region CreateByList - NV giao việc
            var tableCreateBySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableCreateBy = new List<SqlDataRecord>();
            if (searchViewModel.CreateBy != null && searchViewModel.CreateBy.Count > 0)
            {
                foreach (var a in searchViewModel.CreateBy)
                {
                    var tableRow = new SqlDataRecord(tableCreateBySchema);
                    tableRow.SetString(0, a);
                    tableCreateBy.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateBySchema);
                tableCreateBy.Add(tableRow);
            }
            #endregion

            #region ReporterList - NV theo dõi
            var tableReporterSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableReporter = new List<SqlDataRecord>();
            if (searchViewModel.Reporter != null && searchViewModel.Reporter.Count > 0)
            {
                foreach (var a in searchViewModel.Reporter)
                {
                    var tableRow = new SqlDataRecord(tableReporterSchema);
                    tableRow.SetString(0, a);
                    tableReporter.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableReporterSchema);
                tableReporter.Add(tableRow);
            }
            #endregion

            #region AssigneeList - NV được phân công
            var tableAssigneeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableAssignee = new List<SqlDataRecord>();
            if (searchViewModel.Assignee != null && searchViewModel.Assignee.Count > 0)
            {
                foreach (var a in searchViewModel.Assignee)
                {
                    var tableRow = new SqlDataRecord(tableAssigneeSchema);
                    tableRow.SetString(0, a);
                    tableAssignee.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableAssigneeSchema);
                tableAssignee.Add(tableRow);
            }
            #endregion

            #region RolesCodeList - Phòng ban
            //And a table as a list of those records
            var tableRolesCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            var tableRolesCode = new List<SqlDataRecord>();
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                foreach (var p in searchViewModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                    tableRow.SetString(0, p);
                    tableRolesCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion
            string sqlQuery = "EXEC [Report].[usp_TaskGroupReport] @TaskStatusCode, @CreateByCode, @Reporter, @Assignee, @RolesCode, @StartFromDate, @StartToDate, @EstimateEndFromDate, @EstimateEndToDate, @EndFromDate, @EndToDate, @AccountId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableTaskStatusCode
                },
               new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateByCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCreateBy
                },
               new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Reporter",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableReporter
                },
               new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Assignee",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableAssignee
                },
               new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchViewModel.StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchViewModel.StartToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndFromDate",
                    Value = searchViewModel.EstimateEndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndToDate",
                    Value = searchViewModel.EstimateEndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndFromDate",
                    Value = searchViewModel.EndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndToDate",
                    Value = searchViewModel.EndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AccountId",
                    Value = CurrentUser.AccountId
                }
            };

            var data = _context.Database.SqlQuery<TaskGroupReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            
            return data;
        }
        #endregion

        #region Export Excel
        const int startIndex = 8;

        public ActionResult ExportExcel(TaskGroupReportSerchViewModel searchViewModel)
        {
            DateTime? excelFromDate = searchViewModel.FromDate;
            DateTime? excelToDate = searchViewModel.ToDate;
            //DateTime? excelRatioFromDate, excelRatioToDate;
            //get search value to display in file
            var data = GetData(searchViewModel);
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return Export(data, filterDisplayList, searchViewModel.CommonDate, excelFromDate, excelToDate);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<TaskGroupReportViewModel> viewModel, List<SearchDisplayModel> filters, string CommonDate = null, DateTime? excelFromDate = null, DateTime? excelToDate = null, DateTime? excelRatioFromDate = null, DateTime? excelRatioToDate = null)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "Summary ", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Description ", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaskCodeSubTask ", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaskGroupEmployee", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "AssigneeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaskStatusNameSubtask", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "DescriptionSubtask", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StartDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "EstimateEndDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "EndDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "TaskStatusName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "RolesName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CreateByName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CreateDate", isAllowedToEdit = false, isDateTime = true });
            #endregion Master

            //Header
            string fileheader = "BÁO CÁO GIAO VIỆC CHO NHÓM";
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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);
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

    }
}