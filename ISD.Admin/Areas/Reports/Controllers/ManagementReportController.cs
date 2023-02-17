using DevExpress.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml;
using Reports.XReports;
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
    public class ManagementReportController : BaseController
    {
        // GET: ManagementReport
        public ActionResult Index(int id)
        {
            ViewBag.ProfileCode = id;
            return View();
        }

        public ActionResult DocumentViewerPartial(int ProfileCode)
        {
            ViewData["ManagementReport_Report"] = CreateDataReport(ProfileCode);
            ViewBag.ProfileCode = ProfileCode;
            return PartialView("_DocumentViewerPartial");
        }
        public ActionResult DocumentViewerPartialExport(int ProfileCode)
        {
            ManagementReportXtra managementReport = CreateDataReport(ProfileCode);
            return DocumentViewerExtension.ExportTo(managementReport, Request);
        }
        public ManagementReportXtra CreateDataReport(int ProfileCode)
        {
            //Bước 1: Lây data 
            DataSet ds = GetData(ProfileCode);
            //Bước 2: Tạo report
            ManagementReportXtra report = new ManagementReportXtra();
            //Bước 3: Gán data cho report
            report.DataSource = ds;

            //Bước 4: Set các thông số khác cho report
            report.DataMember = "Header";
            report.Name = "Phiếu xuất Report";
            return report;
        }
        public DataSet GetData(int ProfileCode)
        {
            try
            {
                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Report.usp_ManagementReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@ProfileCode", ProfileCode);
                            sda.SelectCommand.Parameters.AddWithValue("@CurrentCompanyCode", CurrentUser.CompanyCode);
                            sda.Fill(ds);
                            ds.Tables[0].TableName = "Header";
                            ds.Tables[1].TableName = "SalesEmployee";
                            ds.Tables[2].TableName = "SpecAnCuong";
                            ds.Tables[3].TableName = "SpecCompetitor";
                            ds.Tables[4].TableName = "ConstructAnCuong";
                            ds.Tables[5].TableName = "ConstructCompetitor";
                            ds.Tables[6].TableName = "Investor";
                            ds.Tables[7].TableName = "ConsultingDesign";
                            ds.Tables[8].TableName = "GeneralContractor";
                            ds.Tables[9].TableName = "ConsultingUnit";
                            ds.Tables[10].TableName = "Activities";
                            ds.Tables[11].TableName = "Construction";
                        }
                    }
                }
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}