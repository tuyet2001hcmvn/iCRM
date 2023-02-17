using DevExpress.Web.Mvc;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ExportCatalogueController : Controller
    {
        // GET: ExportCatalogue
        public ActionResult Index(int id)
        {
            ViewBag.DeliveryCode = id;
            return View();
        }
        
        public ActionResult DocumentViewerPartial(int DeliveryCode)
        {
            ViewData["ExportCatalogue_Report"] = CreateDataReport(DeliveryCode);
            ViewBag.DeliveryCode = DeliveryCode;
            return PartialView("_DocumentViewerPartial");
        }
        public ActionResult DocumentViewerPartialExport(int DeliveryCode)
        {
            CatalogueXtraReport catalogueReport = CreateDataReport(DeliveryCode);
            return DocumentViewerExtension.ExportTo(catalogueReport, Request);
        }

        #region Lấy dữ liệu từ store proc
        //Tạo report
        public CatalogueXtraReport CreateDataReport(int? DelyveryCode = null)
        {
            //Bước 1: Lây data 
            DataSet ds = GetData(DelyveryCode);
            //Bước 2: Tạo report
            CatalogueXtraReport report = new CatalogueXtraReport();
            //Bước 3: Gán data cho report
            report.DataSource = ds;

            //Bước 4: Set các thông số khác cho report
            report.DataMember = "Detail";
            report.Name = "Phiếu xuất catalogue";
            return report;
        }

        public DataSet GetData(int? DeliveryCode = null)
        {
            try
            {
                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Warehouse.Export_Catalogue", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@DeliveryCode", DeliveryCode ?? (object)DBNull.Value);
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