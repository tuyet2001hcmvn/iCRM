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
    public class TaskAnalysisDVKTReportController : BaseController
    {
        // GET: TaskAnalysisDVKTReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            TaskAnalysisDVKTSearchModel searchViewModel = new TaskAnalysisDVKTSearchModel()
            {
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate,
            };
            var searchModel = (TaskAnalysisDVKTSearchModel)TempData[CurrentUser.AccountId + "TaskAnalysisDVKTSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "TaskAnalysisDVKTTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "TaskAnalysisDVKTModeSearch"];
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
            var pageId = GetPageId("/Reports/TaskAnalysisDVKTReport");
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
            CreateViewBagForSearch(searchViewModel);
            return View(searchViewModel);
        }
        private void CreateViewBagForSearch(TaskAnalysisDVKTSearchModel searchViewModel)
        {

            //Dropdown trạng thái
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem { Text = "Đã thực hiện", Value = "completed" });
            result.Add(new SelectListItem { Text = "Lịch hủy", Value = "Lịch hủy" });
            result.Add(new SelectListItem { Text = "Tư vấn qua ĐT", Value = "Tư vấn qua ĐT" });
            result.Add(new SelectListItem { Text = "Lịch hẹn lại", Value = "Lịch hẹn lại" });

            ViewBag.TaskStatusCode = new SelectList(result, "Value", "Text");

            //Dropdown TT bảo hành
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");

            //Dropdown Department
            var rolesList = _context.DepartmentModel.Where(p => p.Actived == true).ToList();
            ViewBag.DepartmentId = new SelectList(rolesList, "DepartmentId", "DepartmentName");
            //Loại bảo hành
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC);
            listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();

            ViewBag.WorkFlowId = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName");
            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);
            ViewBag.PageId = GetPageId("/Reports/TaskAnalysisDVKTReport");
        }

        #region export to excel
        public ActionResult ExportExcel(TaskAnalysisDVKTSearchModel searchModel)
        {
            var reportData = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(reportData ,searchModel, filterDisplayList);
        }

        private List<TaskAnalysisDVKTExcelModel> GetDataSearch(TaskAnalysisDVKTSearchModel searchModel)
        {
            List<TaskAnalysisDVKTExcelModel> result = new List<TaskAnalysisDVKTExcelModel>();

            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Report].[usp_TaskAnalysisDVKTReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchModel.FromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchModel.ToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", searchModel.ServiceTechnicalTeamCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@DepartmentId", searchModel.DepartmentId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", searchModel.WorkFlowId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskStatusCode", searchModel.TaskStatusCode ?? (object)DBNull.Value);
                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                #region Convert to list
                                TaskAnalysisDVKTExcelModel model = new TaskAnalysisDVKTExcelModel();

                                model.WorkFlowName = item["WorkflowName"].ToString();

                                if (!string.IsNullOrEmpty(item["QtyComplete"].ToString()))
                                {
                                    model.QtyComplete = Convert.ToInt32(item["QtyComplete"].ToString());
                                }
                                if (!string.IsNullOrEmpty(item["QtyCancel"].ToString()))
                                {
                                    model.QtyCancel = Convert.ToInt32(item["QtyCancel"].ToString());
                                }
                                if (!string.IsNullOrEmpty(item["QtyAdvisoryPhone"].ToString()))
                                {
                                    model.QtyAdvisoryPhone = Convert.ToInt32(item["QtyAdvisoryPhone"].ToString());
                                }
                                if (!string.IsNullOrEmpty(item["QtyBookLater"].ToString()))
                                {
                                    model.QtyBookLater = Convert.ToInt32(item["QtyBookLater"].ToString());
                                }
                                if (!string.IsNullOrEmpty(item["QtyTotal"].ToString()))
                                {
                                    model.QtyTotal = Convert.ToInt32(item["QtyTotal"].ToString());
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
        private List<TaskAnalysisDVKTExcelModel> GetData(TaskAnalysisDVKTSearchModel searchModel)
        {
            var data = GetDataSearch(searchModel);
            if (searchModel.CommonDate != "Custom")
            {
                searchModel.CurrentFromDate = searchModel.FromDate;
                searchModel.CurrentToDate = searchModel.ToDate;

                DateTime? fromDate = searchModel.FromDate;
                DateTime? toDate = searchModel.ToDate;
                DateTime? previousFromDate;
                DateTime? previousToDate;

                _unitOfWork.CommonDateRepository.GetDateBy(searchModel.CommonDate, out fromDate, out toDate, out previousFromDate, out previousToDate);
                var searchPrevious = new TaskAnalysisDVKTSearchModel
                {
                    FromDate = searchModel.FromDate,
                    ToDate = searchModel.ToDate,
                    ServiceTechnicalTeamCode = searchModel.ServiceTechnicalTeamCode,
                    DepartmentId = searchModel.DepartmentId,
                    WorkFlowId = searchModel.WorkFlowId,
                    TaskStatusCode = searchModel.TaskStatusCode
                };
                searchPrevious.FromDate = previousFromDate;
                searchPrevious.ToDate = previousToDate;
                searchModel.PreviousFromDate = previousFromDate;
                searchModel.PreviousToDate = previousToDate;


                //Lấy dữ liệu kỳ trước
                var previousData = GetDataSearch(searchPrevious);
                if (previousData != null && previousData.Count > 0)
                {
                    for (int i = 0; i < data.Count(); i++)
                    {
                        if (previousData.ElementAt(i).QtyComplete != 0)
                        {
                            decimal KyNay = data.ElementAt(i).QtyComplete;
                            decimal KyTruoc = previousData.ElementAt(i).QtyComplete;
                            decimal rate = KyNay / KyTruoc * 100;
                            data.ElementAt(i).PreviousCompleteRate = rate.ToString("#,##0.##") + " (kỳ trước : " + KyTruoc +")";

                        }
                        else
                        {
                            data.ElementAt(i).PreviousCompleteRate = "N/A (kỳ trước : 0)";
                        }
                        if (previousData.ElementAt(i).QtyCancel != 0)
                        {
                            decimal KyNay = data.ElementAt(i).QtyCancel;
                            decimal KyTruoc = previousData.ElementAt(i).QtyCancel;
                            decimal rate = KyNay / KyTruoc * 100;
                            data.ElementAt(i).PreviousCancelRate = rate.ToString("#,##0.##") + " (kỳ trước : " + KyTruoc + ")";
                        }
                        else
                        {
                            data.ElementAt(i).PreviousCancelRate = "N/A (kỳ trước : 0)";
                        }
                        if (previousData.ElementAt(i).QtyAdvisoryPhone != 0)
                        {
                            decimal KyNay = data.ElementAt(i).QtyAdvisoryPhone;
                            decimal KyTruoc = previousData.ElementAt(i).QtyAdvisoryPhone;
                            decimal rate = KyNay  / KyTruoc * 100;
                            data.ElementAt(i).PreviousAdvisoryPhoneRate = rate.ToString("#,##0.##") + " (kỳ trước : " + KyTruoc + ")";
                        }
                        else
                        {
                            data.ElementAt(i).PreviousAdvisoryPhoneRate = "N/A (kỳ trước : 0)";
                        }
                        if (previousData.ElementAt(i).QtyBookLater != 0)
                        {
                            decimal KyNay = data.ElementAt(i).QtyBookLater;
                            decimal KyTruoc = previousData.ElementAt(i).QtyBookLater;
                            decimal rate = KyNay / KyTruoc * 100;
                            data.ElementAt(i).PreviousBookLaterRate = rate.ToString("#,##0.##") + " (kỳ trước : " + KyTruoc + ")";
                        }
                        else
                        {
                            data.ElementAt(i).PreviousBookLaterRate = "N/A (kỳ trước : 0)";
                        }
                        if (previousData.ElementAt(i).QtyTotal != 0)
                        {
                            decimal KyNay = data.ElementAt(i).QtyTotal;
                            decimal KyTruoc = previousData.ElementAt(i).QtyTotal;
                            decimal rate = KyNay / KyTruoc * 100;
                            data.ElementAt(i).PreviousTotalRate = rate.ToString("#,##0.##") + " (kỳ trước : " + KyTruoc + ")";
                        }
                        else
                        {
                            data.ElementAt(i).PreviousTotalRate = "N/A (kỳ trước : 0)";
                        }
                    }
                }
            }
            return data;
        }

        public ActionResult Export(List<TaskAnalysisDVKTExcelModel> viewModel, TaskAnalysisDVKTSearchModel searchModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            if(searchModel.FromDate == null || searchModel.ToDate == null)
            {
                fileheader = "Báo cáo Phân tích bảo hành DVKT";
            }
            else
            {
                fileheader = "Báo cáo Phân tích bảo hành DVKT " + searchModel.FromDate.Value.ToString("dd/MM/yyyy") + " - " + searchModel.ToDate.Value.ToString("dd/MM/yyyy");
            }
            
            string mergeHeaderTitle="";
            if (searchModel.CommonDate !="Custom")
            {
                mergeHeaderTitle = "Tỷ lệ % so với kỳ " + searchModel.PreviousFromDate.Value.ToString("dd/MM/yyyy") + " - " + searchModel.PreviousToDate.Value.ToString("dd/MM/yyyy");
            }
            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });//2.  Loại bảo hành
            columns.Add(new ExcelTemplate { ColumnName = "QtyComplete", isAllowedToEdit = false, MergeHeaderTitle = "Số lượng", isCurrency = true });  //3. Đã thực hiện 
            columns.Add(new ExcelTemplate { ColumnName = "QtyCancel", isAllowedToEdit = false, MergeHeaderTitle = "Số lượng", isCurrency = true });//4. Lịch hủy
            columns.Add(new ExcelTemplate { ColumnName = "QtyAdvisoryPhone", isAllowedToEdit = false, MergeHeaderTitle = "Số lượng", isCurrency = true });//5. Tư vấn qua điện thoại
            columns.Add(new ExcelTemplate { ColumnName = "QtyBookLater", isAllowedToEdit = false, MergeHeaderTitle = "Số lượng", isCurrency = true });//6. Lịch hẹn lại
            columns.Add(new ExcelTemplate { ColumnName = "QtyTotal", isAllowedToEdit = false, MergeHeaderTitle = "Số lượng", isCurrency = true });//9. Tổng cộng
            if(searchModel.CommonDate !="Custom")
            {
                columns.Add(new ExcelTemplate { ColumnName = "PreviousCompleteRate", isAllowedToEdit = false, MergeHeaderTitle = mergeHeaderTitle });
                columns.Add(new ExcelTemplate { ColumnName = "PreviousCancelRate", isAllowedToEdit = false, MergeHeaderTitle = mergeHeaderTitle});
                columns.Add(new ExcelTemplate { ColumnName = "PreviousAdvisoryPhoneRate", isAllowedToEdit = false, MergeHeaderTitle = mergeHeaderTitle });
                columns.Add(new ExcelTemplate { ColumnName = "PreviousBookLaterRate", isAllowedToEdit = false, MergeHeaderTitle = mergeHeaderTitle });
                columns.Add(new ExcelTemplate { ColumnName = "PreviousTotalRate", isAllowedToEdit = false, MergeHeaderTitle = mergeHeaderTitle });
            }    
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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, headerRowMergeCount: 1);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion
        public ActionResult ViewDetail(TaskAnalysisDVKTSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, TaskAnalysisDVKTSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "TaskAnalysisDVKTModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult TaskAnalysisDVKTPivotGridPartial(Guid? templateId = null, TaskAnalysisDVKTSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TaskAnalysisDVKTReport");
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
                return PartialView("_TaskAnalysisDVKTPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<TaskAnalysisDVKTSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_TaskAnalysisDVKTPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(TaskAnalysisDVKTSearchModel searchViewModel, Guid? templateId)
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
            string fileName = "BAO_CAO_PHAN_TICH_BAO_HANH_DVKT";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(TaskAnalysisDVKTSearchModel searchViewModel)
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
            
            if (!string.IsNullOrEmpty(searchViewModel.ServiceTechnicalTeamCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trung tâm bảo hành";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.ServiceTechnicalTeamCode && s.CatalogTypeCode == ConstCatalogType.ServiceTechnicalTeam).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
           
           
            if (!string.IsNullOrEmpty(searchViewModel.TaskStatusCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trạng thái";
                filter.DisplayValue = searchViewModel.TaskStatusCode;
                filterList.Add(filter);
            }
           
            if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Loại";
                var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC);
                listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
                var value = listWorkFlow.FirstOrDefault(s => s.WorkFlowId == searchViewModel.WorkFlowId).WorkFlowName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.DepartmentId != null && searchViewModel.DepartmentId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phòng ban";
                var dep = _context.DepartmentModel.FirstOrDefault(s => s.DepartmentId == searchViewModel.DepartmentId);
                var value = dep.DepartmentName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}