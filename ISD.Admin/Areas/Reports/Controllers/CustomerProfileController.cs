using DevExpress.Web.Mvc;
using ISD.Core;
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
    public class CustomerProfileController : BaseController
    {
        // GET: CustomerProfile
        //[ISDAuthorizationAttribute]
        public ActionResult Index(string Id, string CompanyCode, string Year)
        {
            ViewBag.ProfileCode = Id;
            ViewBag.CompanyCode = CompanyCode;
            ViewBag.Year = Year;
            return View();
        }
        
        public ActionResult DocumentViewerPartial(string ProfileCode, string CompanyCode, string Year)
        {
            ViewData["CustomerProfile_Report"] = CreateDataReport(ProfileCode, CompanyCode, Year);
            ViewBag.ProfileCode = ProfileCode;
            ViewBag.CompanyCode = CompanyCode;
            ViewBag.Year = Year;
            return PartialView("_DocumentViewerPartial");
        }

        public ActionResult DocumentViewerPartialExport(string ProfileCode, string CompanyCode, string Year)
        {
            CustomerProfileXtraReport catalogueReport = CreateDataReport(ProfileCode, CompanyCode, Year);
            return DocumentViewerExtension.ExportTo(catalogueReport, Request);
        }


        #region Lấy dữ liệu từ store proc
        //Tạo report
        public CustomerProfileXtraReport CreateDataReport(string ProfileCode , string CompanyCode, string Year)
        {
            //Bước 1: Lây data 
            DataSet ds = GetData(ProfileCode, CompanyCode);
            //Lấy data từ SAP
            var profile = _unitOfWork.ProfileRepository.GetProfileBy(ProfileCode);
            if (profile.ProfileForeignCode != null)
            {
                #region CongNo
                var congno = _unitOfWork.SAPReportRepository.GetCongNoProfile(CompanyCode, Year, profile.ProfileForeignCode);
                var tableDoanhSo = ds.Tables.Add("DoanhSo");
                tableDoanhSo.Columns.Add("STT");
                tableDoanhSo.Columns.Add("Year");
                tableDoanhSo.Columns.Add("DoanhSo");
                tableDoanhSo.Columns.Add("ThanhToan");
                tableDoanhSo.Columns.Add("ConLai");
                var index = 0;
                foreach (var item in congno.OrderBy(x => x.Year))
                {
                    index++;
                    item.STT = index;
                    tableDoanhSo.Rows.Add(item.STT, item.Year, item.DoanhSo, item.ThanhToan, item.ConLai);
                }

                #endregion
                #region MuaHang
                var muaHang = _unitOfWork.SAPReportRepository.GetMuaHangProfile(CompanyCode, Year, profile.ProfileForeignCode);
                var tableMuaHang = ds.Tables.Add("MuaHang");
                tableMuaHang.Columns.Add("STT");
                tableMuaHang.Columns.Add("GroupProduct");
                tableMuaHang.Columns.Add("Year1");
                tableMuaHang.Columns.Add("Year2");
                tableMuaHang.Columns.Add("Year3");
                tableMuaHang.Columns.Add("Total");
                tableMuaHang.Columns.Add("Year");
                index = 0;
                foreach (var item in muaHang)
                {
                    index++;
                    item.STT = index;
                    tableMuaHang.Rows.Add(item.STT, item.GroupProductName, item.Year2, item.Year1, item.Year, item.Total, Year);
                }
                #endregion
            }


            //Bước 2: Tạo report
            CustomerProfileXtraReport report = new CustomerProfileXtraReport();
            //Bước 3: Gán data cho report
            report.DataSource = ds;

            //Bước 4: Set các thông số khác cho report
            report.DataMember = "Detail";
            report.Name = "Profile khách hàng";
            return report;
        }

        public DataSet GetData(string ProfileCode, string CompanyCode)
        {
            try
            {
                DataSet ds = new DataSet();
                string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Report.ProfileCustomer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@ProfileCode", ProfileCode ?? (object)DBNull.Value);
                            sda.SelectCommand.Parameters.AddWithValue("@CurrentCompanyCode", CompanyCode ?? (object)DBNull.Value);
                            sda.Fill(ds);
                            ds.Tables[0].TableName = "Detail";
                            ds.Tables[1].TableName = "InCharge";
                            ds.Tables[2].TableName = "Contact";
                            ds.Tables[3].TableName = "ShowroomGTB";
                            ds.Tables[4].TableName = "CustomerCare";
                            ds.Tables[5].TableName = "CustomerService";
                            ds.Tables[6].TableName = "Contractor";
                            ds.Tables[7].TableName = "Address";
                            ds.Tables[8].TableName = "Event";
                            ds.Tables[9].TableName = "CertificateAC";
                            ds.Tables[10].TableName = "Spons";
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