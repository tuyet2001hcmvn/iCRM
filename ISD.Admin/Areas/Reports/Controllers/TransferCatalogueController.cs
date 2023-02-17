using DevExpress.Web.Mvc;
using ISD.Extensions;
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
    public class TransferCatalogueController : Controller
    {
        // GET: TransferCatalogue
        //[ISDAuthorizationAttribute]
        public ActionResult Index(int id)
        {
            ViewBag.TransferCode = id;
            return View();
        }
        
        public ActionResult DocumentViewerPartial(int TransferCode)
        {
            ViewData["TransferCatalogue_Report"] = CreateDataReport(TransferCode);
            ViewBag.TransferCode = TransferCode;
            return PartialView("_DocumentViewerPartial");
        }

        public ActionResult DocumentViewerPartialExport(int TransferCode)
        {
            TransferCatalogueXtraReport catalogueReport = CreateDataReport(TransferCode);
            return DocumentViewerExtension.ExportTo(catalogueReport, Request);
        }


        #region Lấy dữ liệu từ store proc
        //Tạo report
        public TransferCatalogueXtraReport CreateDataReport(int? TransferCode = null)
        {
            //Bước 1: Lây data 
            DataSet ds = GetData(TransferCode);
            //Bước 2: Tạo report
            TransferCatalogueXtraReport report = new TransferCatalogueXtraReport();
            //Bước 3: Gán data cho report
            report.DataSource = ds;

            //Bước 4: Set các thông số khác cho report
            report.DataMember = "Detail";
            report.Name = "Phiếu chuyển catalogue";
            return report;
        }

        public DataSet GetData(int? TransferCode = null)
        {
            try
            {
                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Warehouse.Transfer_Catalogue", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@TransferCode", TransferCode ?? (object)DBNull.Value);
                            sda.Fill(ds);
                            ds.Tables[0].TableName = "HeaderInformation";
                            ds.Tables[1].TableName = "Detail";
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
        #endregion
    }
}