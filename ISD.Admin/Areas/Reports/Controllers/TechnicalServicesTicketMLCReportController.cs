using DevExpress.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
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
    public class TechnicalServicesTicketMLCReportController : BaseController
    {
        // GET: CustomerProfile
        [ISDAuthorizationAttribute]
        public ActionResult Index(TechnicalServicesReportSearchViewModel searchViewModel)
        {
            if (searchViewModel != null)
            {
                if (searchViewModel.FromDate != null && searchViewModel.ToDate != null)
                {
                    searchViewModel.IsCallFirst = false;

                }
                else
                {
                    DateTime? fromDate, toDate;
                    var CommonDate = "ThisMonth";
                    _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
                    searchViewModel = new TechnicalServicesReportSearchViewModel()
                    {
                        //Ngày ghé thăm
                        CommonDate = CommonDate,
                        FromDate = fromDate,
                        ToDate = toDate,
                        IsCallFirst = true
                    };
                }
                
                
            }
            else
            {
                DateTime? fromDate, toDate;
                var CommonDate = "ThisMonth";
                _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
                searchViewModel = new TechnicalServicesReportSearchViewModel()
                {
                    //Ngày ghé thăm
                    CommonDate = CommonDate,
                    FromDate = fromDate,
                    ToDate = toDate,
                    IsCallFirst = true
                };
            }
            CreateViewBag(searchViewModel);
            return View(searchViewModel);
        }

        public ActionResult DocumentViewerPartialFirstCall(TechnicalServicesReportSearchViewModel searchViewModel = null)
        {
            ViewData["TechnicalServices_Report"] = new TechnicalServicesXtraReport();
            return PartialView("_DocumentViewerPartial", searchViewModel);
        }

        public ActionResult DocumentViewerPartial(string jsonReq)
        {
            var searchViewModel = new TechnicalServicesReportSearchViewModel();
            if (!string.IsNullOrEmpty(jsonReq))
            {
                searchViewModel = JsonConvert.DeserializeObject<TechnicalServicesReportSearchViewModel>(jsonReq);
            }

            ViewData["TechnicalServices_Report"] = CreateDataReport(searchViewModel);
            ViewBag.searchViewModel = searchViewModel;
            return PartialView("_DocumentViewerPartial");
        }

        public ActionResult DocumentViewerPartialExport(string jsonReq)
        {
            var searchViewModel = new TechnicalServicesReportSearchViewModel();
            if (!string.IsNullOrEmpty(jsonReq))
            {
                searchViewModel = JsonConvert.DeserializeObject<TechnicalServicesReportSearchViewModel>(jsonReq);
            }
            ViewData["TechnicalServices_Report"] = CreateDataReport(searchViewModel);
            TechnicalServicesXtraReport quarterReport = CreateDataReport(searchViewModel);
            return DocumentViewerExtension.ExportTo(quarterReport);
        }


        #region Lấy dữ liệu từ store proc
        //Tạo report
        public TechnicalServicesXtraReport CreateDataReport(TechnicalServicesReportSearchViewModel searchViewModel)
        {
            //Bước 1: Tạo report
            TechnicalServicesXtraReport report = new TechnicalServicesXtraReport();
            if (searchViewModel.IsCallFirst == true) return report;


            //Bước 2: Lây data 
            DataSet ds = GetData(searchViewModel);
            //Bước 3: Gán data cho report
            report.DataSource = ds;

            //Bước 4: Set các thông số khác cho report
            report.DataMember = "Detail";
            report.Name = "Tổng Lịch DVKT";
            return report;
        }

        public DataSet GetData(TechnicalServicesReportSearchViewModel searchViewModel)
        {
            
                #region Loại
                //Nếu chọn loại "Tất cả" -> chỉ lấy các loại có trong list filter
                var workflowList = new List<Guid>();
                if (searchViewModel.WorkFlowId != null && searchViewModel.WorkFlowId.Count > 0)
                {
                    foreach (var item in searchViewModel.WorkFlowId)
                    {
                        workflowList.Add((Guid)item);
                    }
                }
                else
                {
                    var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC, CurrentUser.CompanyCode);
                    if (listWorkFlow != null && listWorkFlow.Count > 0)
                    {
                        workflowList = listWorkFlow.Select(p => p.WorkFlowId).ToList();
                    }
                }

                #region WorkflowList
                //Build your record
                var tableWorkFlowSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
                }.ToArray();

                //And a table as a list of those records
                var tableWorkFlow = new List<SqlDataRecord>();
                if (workflowList != null && workflowList.Count > 0)
                {
                    foreach (var r in workflowList)
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
                #endregion
                #region Trạng thái
                //Build your record
                var tableTaskStatusCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 50)
                }.ToArray();

                //And a table as a list of those records
                var tableTaskStatusCode = new List<SqlDataRecord>();
                List<string> TaskStatusCodeLst = new List<string>();
                if (searchViewModel.TaskStatusCode != null && searchViewModel.TaskStatusCode.Count > 0)
                {
                    foreach (var r in searchViewModel.TaskStatusCode)
                    {
                        var tableRow = new SqlDataRecord(tableTaskStatusCodeSchema);
                        tableRow.SetString(0, r);
                        if (!TaskStatusCodeLst.Contains(r))
                        {
                            TaskStatusCodeLst.Add(r);
                            tableTaskStatusCode.Add(tableRow);
                        }
                    }
                }
                else
                {
                    var tableRow = new SqlDataRecord(tableTaskStatusCodeSchema);
                    tableTaskStatusCode.Add(tableRow);
                }
                #endregion
                #region Phòng ban
                //Build your record
                var tableRolesCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 50)
                }.ToArray();

                //And a table as a list of those records
                var tableRolesCode = new List<SqlDataRecord>();
                List<string> RolesCodeLst = new List<string>();
                if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
                {
                    foreach (var r in searchViewModel.RolesCode)
                    {
                        var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                        tableRow.SetString(0, r);
                        if (!RolesCodeLst.Contains(r))
                        {
                            RolesCodeLst.Add(r);
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
                #region Loại
                //Build your record
                var tableServiceTechnicalTeamCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 50)
                }.ToArray();

                //And a table as a list of those records
                var tableServiceTechnicalTeamCode = new List<SqlDataRecord>();
                List<string> ServiceTechnicalTeamCodeLst = new List<string>();
                if (searchViewModel.ServiceTechnicalTeamCode != null && searchViewModel.ServiceTechnicalTeamCode.Count > 0)
                {
                    foreach (var r in searchViewModel.ServiceTechnicalTeamCode)
                    {
                        var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                        tableRow.SetString(0, r);
                        if (!ServiceTechnicalTeamCodeLst.Contains(r))
                        {
                            ServiceTechnicalTeamCodeLst.Add(r);
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

                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Report.usp_CountTask_Ticket_MLC_Report", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            var tablework = sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", tableWorkFlow);
                            tablework.SqlDbType = SqlDbType.Structured;
                            tablework.TypeName = "[dbo].[WorkFlowIdList]";
                            var tableStatus = sda.SelectCommand.Parameters.AddWithValue("@TaskStatusCode", tableTaskStatusCode);
                            tableStatus.SqlDbType = SqlDbType.Structured;
                            tableStatus.TypeName = "[dbo].[StringList]";
                            var tablerolesCode = sda.SelectCommand.Parameters.AddWithValue("@RolesCode", tableRolesCode);
                            tablerolesCode.SqlDbType = SqlDbType.Structured;
                            tablerolesCode.TypeName = "[dbo].[StringList]";
                            var tableserviceTechnicalTeamCode = sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", tableServiceTechnicalTeamCode);
                            tableserviceTechnicalTeamCode.SqlDbType = SqlDbType.Structured;
                            tableserviceTechnicalTeamCode.TypeName = "[dbo].[StringList]";
                            sda.SelectCommand.Parameters.AddWithValue("@FromDate", searchViewModel.FromDate ?? (object)DBNull.Value);
                            sda.SelectCommand.Parameters.AddWithValue("@ToDate", searchViewModel.ToDate ?? (object)DBNull.Value);
                            sda.Fill(ds);
                            ds.Tables[0].TableName = "Master";
                            ds.Tables[1].TableName = "Detail";
                        }
                    }
                }
                return ds;
        }

        public void CreateViewBag(TechnicalServicesReportSearchViewModel searchViewModel) {
            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);

            #region Loại
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET_MLC, CurrentUser.CompanyCode);
            List<SelectListItem> lstWorkFlowItem = new List<SelectListItem>();
            foreach (var item in listWorkFlow)
            {
                var i = new SelectListItem()
                {
                    Text = item.WorkFlowName,
                    Value = item.WorkFlowId.ToString(),
                };
                if (searchViewModel.WorkFlowId != null)
                {
                    i.Selected = searchViewModel.WorkFlowId.Contains(item.WorkFlowId);
                }
                lstWorkFlowItem.Add(i);
            }
            ViewBag.WorkFlowId = lstWorkFlowItem;
            #endregion

            #region Trạng thái
            var result = (from wf in _context.WorkFlowModel
                          join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                          where wf.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET_MLC
                          select new
                          {
                              ts.OrderIndex,
                              ts.TaskStatusCode,
                              ts.TaskStatusName
                          }).Distinct().OrderBy(p => p.OrderIndex).ToList();
            List<SelectListItem> lstTaskStatusCodeItem = new List<SelectListItem>();
            foreach (var item in result)
            {
                var i = new SelectListItem()
                {
                    Text = item.TaskStatusName,
                    Value = item.TaskStatusCode.ToString(),
                };
                if (searchViewModel.TaskStatusCode != null)
                {
                    i.Selected = searchViewModel.TaskStatusCode.Contains(item.TaskStatusCode);
                }
                lstTaskStatusCodeItem.Add(i);
            }
            ViewBag.TaskStatusCode = lstTaskStatusCodeItem;
            //ViewBag.TaskStatusCode = new SelectList(result, "TaskStatusCode", "TaskStatusName");

            #endregion

            #region Phòng ban
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
                //ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");

            List<SelectListItem> lstRolesCodeItem = new List<SelectListItem>();
            foreach (var item in rolesList)
            {
                var i = new SelectListItem()
                {
                    Text = item.RolesName,
                    Value = item.RolesCode.ToString(),
                };
                if (searchViewModel.RolesCode != null)
                {
                    i.Selected = searchViewModel.RolesCode.Contains(item.RolesCode);
                }
                lstRolesCodeItem.Add(i);
            }
            ViewBag.RolesCode = lstRolesCodeItem;
            #endregion

            #region //Get list ServiceTechnicalTeamCode (Trung tâm bảo hành)
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
                //ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");
            List<SelectListItem> lstServiceTechnicalTeamCodeItem = new List<SelectListItem>();
                foreach (var item in serviceTechnicalTeamCodeList)
            {
                var i = new SelectListItem()
                {
                    Text = item.CatalogText_vi,
                    Value = item.CatalogCode.ToString(),
                };
                if (searchViewModel.ServiceTechnicalTeamCode != null)
                {
                    i.Selected = searchViewModel.ServiceTechnicalTeamCode.Contains(item.CatalogCode);
                }
                lstServiceTechnicalTeamCodeItem.Add(i);
            }
            ViewBag.ServiceTechnicalTeamCode = lstServiceTechnicalTeamCodeItem;
            #endregion
        }
        #endregion
    }
}