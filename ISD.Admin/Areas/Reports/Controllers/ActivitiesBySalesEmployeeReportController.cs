using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ActivitiesBySalesEmployeeReportController : BaseController
    {
        // GET: ActivitiesBySalesEmployeeReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (ActivitiesBySalesEmployeeReportSearchViewModel)TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeModeSearch"];
            var pageId = GetPageId("/Reports/ActivitiesBySalesEmployeeReport");

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
            CreateViewBag(ConstWorkFlowCategory.ACTIVITIES);
            if (searchModel == null)
            {
                DateTime? fromDate, toDate;
                var CommonDate = "ThisMonth";
                _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
                searchModel = new ActivitiesBySalesEmployeeReportSearchViewModel()
                {
                    //Ngày ghé thăm
                    CommonDate = CommonDate,
                    CommonDate2 = "Custom",
                    StartFromDate = fromDate,
                    StartToDate = toDate,
                };

            }
            #region CommonDate
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchModel.CommonDate);

            //Common Date 2
            var commonDateList2 = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate2 = new SelectList(commonDateList2, "CatalogCode", "CatalogText_vi", searchModel.CommonDate2);
            #endregion

            return View(searchModel);
        }
        public void CreateViewBag(string Type)
        {
            //Type: Loại (WorkflowCategoryCode)
            ViewBag.Type = Type;

            #region //WorkFlowId - Loại
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type);
            listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
            ViewBag.WorkFlowIdList = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName");
            #endregion
            

            #region //TaskStatusCode - Trạng thái
            var isShowTaskStatusCode = true;
            if (Type == ConstWorkFlowCategory.MyFollow || Type == ConstWorkFlowCategory.MyWork)
            {
                isShowTaskStatusCode = false;
            }
            else
            {
                var result = (from wf in _context.WorkFlowModel
                              join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                              where wf.WorkflowCategoryCode == Type
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
            }
            ViewBag.isShowTaskStatusCode = isShowTaskStatusCode;
            #endregion

            #region //Get list SalesEmployeeCode (NV kinh doanh)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesEmployeeCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion
        }
        #endregion

        #region Export Excel
        const int startIndex = 8;

        public ActionResult ExportExcel(ActivitiesBySalesEmployeeReportSearchViewModel searchModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            var result = GetData(searchModel);
            return Export(result, filterDisplayList);
        }
        private List<ActivitiesBySalesEmployeeReportViewModel> GetData(ActivitiesBySalesEmployeeReportSearchViewModel searchModel)
        {
            #region SalesEmployeeCode

            //Build your record
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("StringList", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            if (searchModel.SalesEmployeeCodes != null && searchModel.SalesEmployeeCodes.Count > 0)
            {
                foreach (var r in searchModel.SalesEmployeeCodes)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                    tableRow.SetString(0, r);
                    tableSalesEmployeeCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }

            #endregion

            #region WorkflowList
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("GuidList", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (searchModel.WorkFlowIdLists != null && searchModel.WorkFlowIdLists.Count > 0)
            {
                foreach (var r in searchModel.WorkFlowIdLists)
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

            string sqlQuery = "EXEC [Report].[usp_ActivitiesBySalesEmployeeReport] @SalesEmployeeCode, @WorkFlowIdList, @CurrentCompanyCode, @StartFromDate, @StartToDate, @CreateFromDate, @CreateToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowIdList",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableWorkFlow
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentUser.CompanyCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchModel.StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchModel.StartToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateFromDate",
                    Value = searchModel.CreateFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateToDate",
                    Value = searchModel.CreateToDate ?? (object)DBNull.Value
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<ActivitiesBySalesEmployeeReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }
        public FileContentResult Export(List<ActivitiesBySalesEmployeeReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO CHI TIẾT HOẠT ĐỘNG CSKH THEO NHÂN VIÊN";
            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SalesEmployeeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "RolesName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Summary", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Description", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaskStatusName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StartDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false, isDateTime = true });

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
        public ActionResult ViewDetail(ActivitiesBySalesEmployeeReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ActivitiesBySalesEmployeeReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ActivitiesBySalesEmployeeModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ActivitiesBySalesEmployeeGridPartial(Guid? templateId = null, ActivitiesBySalesEmployeeReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ActivitiesBySalesEmployeeReport");
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
                return PartialView("_ActivitiesBySalesEmployeeGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ActivitiesBySalesEmployeeReportSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ActivitiesBySalesEmployeeGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ActivitiesBySalesEmployeeReportSearchViewModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_CHI_TIET_HOAT_DONG_CSKH_THEO_NHAN_VIEN";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ActivitiesBySalesEmployeeReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.CreateFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo từ ngày", DisplayValue = searchViewModel.CreateFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CreateToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo đến ngày", DisplayValue = searchViewModel.CreateToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Bắt đầu từ ngày", DisplayValue = searchViewModel.StartFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Bắt đầu đến ngày", DisplayValue = searchViewModel.StartToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.SalesEmployeeCodes != null && searchViewModel.SalesEmployeeCodes.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên kinh doanh";
                foreach (var code in searchViewModel.SalesEmployeeCodes)
                {
                    var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == code);
                    filter.DisplayValue += sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.WorkFlowIdLists != null && searchViewModel.WorkFlowIdLists.Count > 0 && searchViewModel.WorkFlowIdLists[0] != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Loại hoạt động";
                var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.ACTIVITIES);
                listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
                foreach (var wf in searchViewModel.WorkFlowIdLists)
                {
                    var workFlow = listWorkFlow.FirstOrDefault(s => s.WorkFlowId == wf);
                    filter.DisplayValue += workFlow.WorkFlowName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}