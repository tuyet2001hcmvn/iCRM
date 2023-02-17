using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WF_SyncData
{
    public partial class frmSyncData : Form
    {
        EntityDataContext _context = new EntityDataContext();
        public Guid SYSTEM;
        public static readonly int FirstSync = Convert.ToInt32(ConfigurationManager.AppSettings["FirstSync"]);
        private static string appPath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static string logFilePath = System.IO.Path.Combine(appPath, DateTime.Now.ToString("yyyyMMdd") + "-SyncDataFromSAP" + ".log");
        private static readonly bool isDebug = Convert.ToBoolean(ConfigurationManager.AppSettings["isDebug"]);
        public static readonly int FromDateSyncProfile = Convert.ToInt32(ConfigurationManager.AppSettings["FromDateSyncProfile"]);
        public static readonly int ToDateSyncProfile = Convert.ToInt32(ConfigurationManager.AppSettings["ToDateSyncProfile"]);
        public frmSyncData()
        {
            InitializeComponent();
            SYSTEM = new Guid("FD68F5F8-01F9-480F-ACB7-BA5D74D299C8");
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //Master data
            var timer1 = ConfigurationManager.AppSettings["Timer1"];
            this.timerSyncMasterData.Interval = int.Parse(timer1);
            //Profile
            var timer2 = ConfigurationManager.AppSettings["Timer2"];
            this.timerSyncProfile.Interval = int.Parse(timer2);
            //Contact
            var timerContact = ConfigurationManager.AppSettings["TimerContact"];
            this.timerSyncContact.Interval = int.Parse(timerContact);
            //Material
            var timer3 = ConfigurationManager.AppSettings["Timer3"];
            this.timerSyncMaterial.Interval = int.Parse(timer3);
            //Material detail
            var timer4 = ConfigurationManager.AppSettings["Timer4"];
            this.timerSyncMaterialDetail.Interval = int.Parse(timer4);
        }

        #region Processing
        private void btnSyncData_Click(object sender, EventArgs e)
        {
            if (btnSyncData.Text == "SYNC MASTER DATA")
            {
                btnSyncData.Text = "PROCESSING";
                btnSyncData.BackColor = Color.DarkRed;

                RunFunction(0);
                RunFunction(11);
                RunFunction(12);
                timerSyncMasterData.Start();
            }
            else if (btnSyncData.Text == "PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncMasterData.Stop();
                    btnSyncData.Text = "SYNC MASTER DATA";
                    btnSyncData.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        private void btnSyncProfile_Click(object sender, EventArgs e)
        {
            if (btnSyncProfile.Text == "SYNC PROFILE")
            {
                btnSyncProfile.Text = "PROCESSING";
                btnSyncProfile.BackColor = Color.DarkRed;

                WriteLogFile(logFilePath, "SYNC PROFILE: Start.");
                RunFunction(1);
                timerSyncProfile.Start();
            }
            else if (btnSyncProfile.Text == "PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncProfile.Stop();
                    btnSyncProfile.Text = "SYNC PROFILE";
                    btnSyncProfile.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        private void btnSyncContact_Click(object sender, EventArgs e)
        {
            if (btnSyncContact.Text == "SYNC CONTACT")
            {
                btnSyncContact.Text = "PROCESSING";
                btnSyncContact.BackColor = Color.DarkRed;

                WriteLogFile(logFilePath, "SYNC CONTACT: Start.");
                RunFunction(14);
                timerSyncContact.Start();
            }
            else if (btnSyncContact.Text == "PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncContact.Stop();
                    btnSyncContact.Text = "SYNC CONTACT";
                    btnSyncContact.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        private void btnSyncMaterial_Click(object sender, EventArgs e)
        {
            if (btnSyncMaterial.Text == "SYNC MATERIAL")
            {
                btnSyncMaterial.Text = "PROCESSING";
                btnSyncMaterial.BackColor = Color.DarkRed;

                //ParentCategory
                SyncMaterial(2);
                //Category
                SyncMaterial(3);
                //Material
                SyncMaterial(1);

                timerSyncMaterial.Start();
            }
            else if (btnSyncMaterial.Text == "PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncMaterial.Stop();
                    btnSyncMaterial.Text = "SYNC MATERIAL";
                    btnSyncMaterial.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        private void btnSyncMaterialDetails_Click(object sender, EventArgs e)
        {
            if (btnSyncMaterialDetails.Text == "SYNC MATERIAL DETAILS")
            {
                btnSyncMaterialDetails.Text = "PROCESSING";
                btnSyncMaterialDetails.BackColor = Color.DarkRed;

                //Product Level
                SyncMaterial(4);
                //Product Color
                SyncMaterial(5);

                timerSyncMaterialDetail.Start();
            }
            else if (btnSyncMaterialDetails.Text == "PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncMaterialDetail.Stop();
                    btnSyncMaterialDetails.Text = "SYNC MATERIAL DETAILS";
                    btnSyncMaterialDetails.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FirstSync != 1)
            {
                //Catalog
                RunFunction(0);
                //Sale Employee
                RunFunction(11);
                //Role
                RunFunction(12);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (FirstSync != 1)
            {
                //Profile
                RunFunction(1);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (FirstSync != 1)
            {
                //ParentCategory
                SyncMaterial(2);
                //Category
                SyncMaterial(3);
                //Material
                SyncMaterial(1);
            }
        }

        private void timerContact_Tick(object sender, EventArgs e)
        {
            if (FirstSync != 1)
            {
                //Contact
                RunFunction(14);
            }
        }


        private void timerSyncMaterialDetail_Tick(object sender, EventArgs e)
        {
            //Product Level
            SyncMaterial(4);
            //Product Color
            SyncMaterial(5);
        }
        #endregion

        #region Run function sync data
        public void RunFunction(int type = 0)
        {
            //Catalog
            if (type == 0)
            {
                for (int i = 2; i <= 8; i++)
                {
                    if (i != 7)
                    {
                        var dt = GetData(i);
                        SyncDataFromSAP(dt, i);
                    }
                }
            }
            //Sale Employee
            else if (type == 11)
            {
                var dt = GetData(type);
                SyncEmployee(dt);
            }
            //Role
            else if (type == 12)
            {
                var dt = GetData(type);
                SyncRoles(dt);
            }
            //Profile
            else if (type == 1)
            {
                var dt = new DataTable();
                var compLst = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).Select(p => p.CompanyCode).ToList();
                if (compLst != null && compLst.Count > 0)
                {
                    foreach (var item in compLst)
                    {
                        var dtItem = GetData(type, item);
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            //var rows = dt.Rows.Count;
                            //dt.Merge(dtItem);
                            SyncProfileFromSAP(dtItem, item);
                        }
                    }
                }
                //SyncProfileFromSAP(dt);

                //Credit Limit
                //var dt_credit = GetDataCreditLimit();
                //SyncCreditLimit(dt_credit);
            }

            //Thiếu type = 1 && code.length > 8 là addressbook (chưa đồng bộ)
            //Thiếu type = 14 là contact (chưa đồng bộ)
            else if (type == 14)
            {
                var dt = new DataTable();
                //var compLst = _context.CompanyModel.Where(p => p.Actived == true).Select(p => p.CompanyCode).ToList();
                //if (compLst != null && compLst.Count > 0)
                //{
                //    foreach (var item in compLst)
                //    {
                //        var dtItem = GetData(type, item);
                //        if (dtItem != null && dtItem.Rows.Count > 0)
                //        {
                //            var rows = dt.Rows.Count;
                //            dt.Merge(dtItem);
                //        }
                //    }
                //}

                dt = GetData(type, "1000");

                SyncContactFromSAP(dt);
                WriteLogFile(logFilePath, "SYNC CONTACT: Done.");
            }
            //Type = 15 => đồng bộ KH theo phân nhóm KH. VD: Z012
            else if (type == 15)
            {
                var dt = new DataTable();
                //var compLst = _context.CompanyModel.Where(p => p.Actived == true).Select(p => p.CompanyCode).ToList();
                //if (compLst != null && compLst.Count > 0)
                //{
                //    foreach (var item in compLst)
                //    {
                //        var dtItem = GetData(type, item);
                //        if (dtItem != null && dtItem.Rows.Count > 0)
                //        {
                //            var rows = dt.Rows.Count;
                //            dt.Merge(dtItem);
                //        }
                //    }
                //}
                dt = GetData(type, "1000");
                SyncProfileWithCAGFromSAP(dt);
            }
        }
        #endregion

        #region Get DataTable from SAP
        public DataTable GetData(int type, string CompanyCode = null)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DATALIST);

            //Thông số truyền vào
            if (type != 1 && type != 14)
            {
                if (type == 15)
                {
                    string CAG = ConfigurationManager.AppSettings["CAG"].ToString();
                    function.SetValue("IM_TYPE", type);
                    function.SetValue("IM_FRDATE", DateTime.Now.ToString("yyyyMMdd"));
                    function.SetValue("IM_TODATE", DateTime.Now.ToString("yyyyMMdd"));
                    function.SetValue("IM_BUKRS", 1000);
                    function.SetValue("IM_VKORG", CAG);
                    function.SetValue("IM_KUNNR", "%");
                    function.SetValue("IM_TAXNO", "%");
                }
                else
                {
                    function.SetValue("IM_TYPE", type);
                    function.SetValue("IM_FRDATE", DateTime.Now.ToString("yyyyMMdd"));
                    function.SetValue("IM_TODATE", DateTime.Now.ToString("yyyyMMdd"));
                    function.SetValue("IM_BUKRS", 1000);
                    function.SetValue("IM_VKORG", 1000);
                }
            }
            else
            {
                var newdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                //FromDate
                var fromDateSyncProfile = -FromDateSyncProfile;
                var fromDate = newdate.AddDays(fromDateSyncProfile);
                if (FirstSync == 1)
                {
                    fromDate = new DateTime(2019, 01, 12);
                }
                //ToDate
                var toDate = DateTime.Now;
                if (ToDateSyncProfile > 0)
                {
                    var toDateSyncProfile = -ToDateSyncProfile;
                    toDate = newdate.AddDays(toDateSyncProfile);
                }
                function.SetValue("IM_TYPE", type);
                function.SetValue("IM_FRDATE", fromDate.ToString("yyyyMMdd"));
                function.SetValue("IM_TODATE", toDate.ToString("yyyyMMdd"));
                function.SetValue("IM_BUKRS", CompanyCode);
                function.SetValue("IM_VKORG", 1000);
                //function.SetValue("IM_KUNNR", "17100333");
            }
            //function.SetValue("IM_KUNNR", 1);
            //function.SetValue("IM_TAXNO", 1);
            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("DATA_T").ToDataTable("DATA_T");
            return datatable;
        }
        public DataTable GetDataCreditLimit()
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_LEVEL_DEBIT);

            //Thông số truyền vào
            function.SetValue("IM_FRDATE", DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            function.SetValue("IM_FRDATE", DateTime.Now.ToString("yyyyMMdd"));
            //function.SetValue("IM_WERKS", 1000);
            //function.SetValue("IM_VKORG", 1000);
            //function.SetValue("IM_KUNNR", "");

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("DEBIT_T").ToDataTable("DEBIT_T");
            return datatable;
        }
        public DataTable GetDataMaterial(int type, string CompanyCode, string MaterialType)
        {
            DataTable datatable = new DataTable();
            try
            {
                //Khởi tạo thư viện và kết nối
                var _sap = new SAPRepository();
                var destination = _sap.GetRfcWithConfig();
                //Định nghĩa hàm cần gọi
                var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_MATERIAL);

                //Thông số truyền vào
                function.SetValue("IM_TYPE", type);
                function.SetValue("IM_FRDATE", DateTime.Now.AddDays(-2).ToString("yyyyMMdd"));
                function.SetValue("IM_FRDATE", DateTime.Now.ToString("yyyyMMdd"));
                function.SetValue("IM_WERKS", CompanyCode);
                function.SetValue("IM_MTART", MaterialType);
                function.SetValue("IM_MATNR", "%");

                //Thực thi
                function.Invoke(destination);

                datatable = function.GetTable("MAT_T").ToDataTable("MAT_T");
                //Nếu là sản phẩm mới add theo công ty
                if (type == 1 && datatable != null && datatable.Rows.Count > 0)
                {
                    var CompanyId = _context.CompanyModel.Where(p => p.CompanyCode == CompanyCode).Select(p => p.CompanyId).FirstOrDefault();
                    datatable.Columns.Add(new DataColumn() { ColumnName = "CompanyId", DefaultValue = CompanyId });
                }
            }
            catch { }
            return datatable;
        }
        #endregion

        #region Sync Master Data Catalog
        private void SyncDataFromSAP(DataTable dataTable, int type)
        {
            var lst = new List<CatalogModel>();
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    string strType = type.ToString();
                    var catalogType = _context.CatalogTypeModel.Where(p => p.Note == strType).Select(p => p.CatalogTypeCode).FirstOrDefault();

                    foreach (DataRow item in dataTable.Rows)
                    {
                        CatalogModel model = new CatalogModel();
                        model.CatalogId = Guid.NewGuid();
                        model.CatalogTypeCode = catalogType;

                        string CatalogCode = "";
                        string CatalogText_vi = "";
                        switch (type)
                        {
                            //CustomerAccountGroup
                            case 2:
                                CatalogCode = item["KTOKD"].ToString();
                                CatalogText_vi = item["CAGN"].ToString();
                                break;
                            //CustomerGroup
                            case 3:
                                CatalogCode = item["KDGRP"].ToString();
                                CatalogText_vi = item["CUSGROUP"].ToString();
                                break;
                            //PaymentTerm
                            case 4:
                                CatalogCode = item["ZTERM"].ToString();
                                CatalogText_vi = item["PAYTERM"].ToString();
                                break;
                            //CustomerAccountAssignmentGroup
                            case 5:
                                CatalogCode = item["KTGRD"].ToString();
                                CatalogText_vi = item["CAAG"].ToString();
                                break;
                            //CashMgmtGroup
                            case 6:
                                CatalogCode = item["FDGRV"].ToString();
                                CatalogText_vi = item["CMG"].ToString();
                                break;
                            //ReconcileAccount
                            case 7:
                                CatalogCode = item["AKONT"].ToString();
                                CatalogText_vi = item["RECACCOUNT"].ToString();
                                break;
                            //SaleOffice
                            case 8:
                                CatalogCode = item["VKBUR"].ToString();
                                CatalogText_vi = item["VKBURTXT"].ToString();
                                break;
                            default:
                                break;
                        }
                        model.CatalogCode = CatalogCode;
                        model.CatalogText_vi = CatalogText_vi;
                        model.Actived = true;

                        var exist = _context.CatalogModel
                                            .Where(p => p.CatalogTypeCode == catalogType && p.CatalogCode == CatalogCode).FirstOrDefault();
                        if (exist == null)
                        {
                            _context.Entry(model).State = EntityState.Added;
                        }
                        else
                        {
                            exist.CatalogText_vi = CatalogText_vi;
                            _context.Entry(exist).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        mess = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        mess = ex.InnerException.Message;
                    }
                }
                //ghi log
                WriteLogFile(logFilePath, "Sync Data error: " + mess);
            }
        }
        #endregion

        #region Sync Sale Employee
        public void SyncEmployee(DataTable dataTable)
        {
            var lst = new List<SalesEmployeeModel>();
            var tempLst = new List<string>();
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var cnStr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        var SalesEmployeeCode = item["PERNR"].ToString();
                        if (!tempLst.Contains(SalesEmployeeCode))
                        {
                            var exist = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == SalesEmployeeCode).FirstOrDefault();
                            if (exist == null)
                            {
                                SalesEmployeeModel model = new SalesEmployeeModel();
                                model.SalesEmployeeCode = SalesEmployeeCode;
                                model.SalesEmployeeName = item["ENAME"].ToString();
                                model.Email = item["RUFNM"].ToString();
                                model.Phone = item["GBORT"].ToString();
                                model.Actived = true;
                                model.CreateBy = SYSTEM;
                                model.CreateTime = DateTime.Now;
                                _context.Entry(model).State = EntityState.Added;
                                //lst.Add(model);
                                _context.SaveChanges();
                            }
                            else
                            {
                                var query = "UPDATE tMasterData.SalesEmployeeModel SET SalesEmployeeName=@p1, Email=@p2, Phone=@p3, LastEditBy=@p4, LastEditTime=@p5 WHERE SalesEmployeeCode=@p6";

                                SqlConnection connection = new SqlConnection(cnStr);
                                connection.Open();

                                SqlCommand cmd = connection.CreateCommand();
                                cmd.CommandText = query;
                                cmd.Parameters.AddWithValue("@p1", item["ENAME"].ToString());
                                cmd.Parameters.AddWithValue("@p2", item["RUFNM"].ToString());
                                cmd.Parameters.AddWithValue("@p3", item["GBORT"].ToString());
                                cmd.Parameters.AddWithValue("@p4", SYSTEM);
                                cmd.Parameters.AddWithValue("@p5", DateTime.Now);
                                cmd.Parameters.AddWithValue("@p6", SalesEmployeeCode);
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                            tempLst.Add(SalesEmployeeCode);
                        }
                    }
                    //if (lst != null && lst.Count > 0)
                    //{
                    //    _context.SalesEmployeeModel.AddRange(lst);
                    //}
                    //_context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        mess = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        mess = ex.InnerException.Message;
                    }
                }
                //ghi log
                //MessageBox.Show(mess);
                WriteLogFile(logFilePath, "Sync Data error: " + mess);
            }
        }
        #endregion

        #region Sync Roles
        public void SyncRoles(DataTable dataTable)
        {
            var lst = new List<RolesModel>();
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    int index = 20;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        var RolesCode = item["ZP_DEPT"].ToString();
                        var exist = _context.RolesModel.Where(p => p.RolesCode == RolesCode).FirstOrDefault();
                        if (exist == null)
                        {
                            RolesModel model = new RolesModel();
                            model.RolesId = Guid.NewGuid();
                            model.RolesCode = item["ZP_DEPT"].ToString();
                            model.RolesName = item["ZP_NAME"].ToString();
                            model.isEmployeeGroup = true;
                            model.OrderIndex = index;
                            model.Actived = true;
                            _context.Entry(model).State = EntityState.Added;
                            _context.SaveChanges();
                        }
                        else
                        {
                            //exist.RolesName = item["ZP_NAME"].ToString();
                            //_context.Entry(exist).State = EntityState.Modified;
                        }
                        index = index + 10;
                    }

                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        mess = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        mess = ex.InnerException.Message;
                    }
                }
                //ghi log
                //MessageBox.Show(mess);
                WriteLogFile(logFilePath, "Sync Data error: " + mess);
            }
        }
        #endregion

        #region Sync Profile
        public void SyncProfileFromSAP(DataTable dataTable, string Company)
        {
            var lst = new List<ProfileModel>();
            WriteLogFile(logFilePath, string.Format("Profile Count: {0}, CompanyCode: {1}", dataTable.Rows.Count, Company));
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                //var dt = dataTable.Select().CopyToDataTable();
                //var existCodeList = _context.ExistProfileModel.Select(p => p.CompanyCode + "_" + p.ProfileForeignCode).AsNoTracking().ToList();
                //if (existCodeList != null && existCodeList.Count > 0)
                //{
                //    var rows = dt.AsEnumerable().Where(p => !existCodeList.Contains(p.Field<string>("BUKRS") + "_" + p.Field<string>("KUNNR")));
                //    if (rows.Any())
                //    {
                //        dt = rows.CopyToDataTable();
                //    }
                //    else
                //    {
                //        dt = new DataTable();
                //    }
                //}
                //else
                //{
                //    var rows = dt.AsEnumerable();
                //    if (rows.Any())
                //    {
                //        dt = rows.CopyToDataTable();
                //    }
                //    else
                //    {
                //        dt = new DataTable();
                //    }
                //}
                //if (dt != null && dt.Rows.Count > 0)
                //{
                try
                {
                    //Tỉnh thành
                    var provinceLst = _context.ProvinceModel.Where(p => p.Actived == true).AsNoTracking().ToList();
                    //Quận huyện
                    var districtLst = _context.DistrictModel.Where(p => p.Actived == true && p.ProvinceId != null && p.DistrictName != null).AsNoTracking().ToList();
                    //Phường xã
                    var wardLst = _context.WardModel.Where(p => p.DistrictId != null && p.WardName != null).AsNoTracking().ToList();
                    //Nhân viên (SalesEmployee)
                    var employeeList = _context.SalesEmployeeModel.Select(p => p.SalesEmployeeCode).AsNoTracking().ToList();

                    //_context.Configuration.AutoDetectChangesEnabled = false;
                    List<ProfileModel> profileList = new List<ProfileModel>();
                    List<string> profileCodeList = new List<string>();
                    List<ProfilePhoneModel> phoneList = new List<ProfilePhoneModel>();
                    List<ProfileBAttributeModel> bAttrList = new List<ProfileBAttributeModel>();
                    List<ProfileContactAttributeModel> contactList = new List<ProfileContactAttributeModel>();
                    List<PersonInChargeModel> personList = new List<PersonInChargeModel>();
                    List<RoleInChargeModel> roleList = new List<RoleInChargeModel>();
                    List<AddressBookModel> addressList = new List<AddressBookModel>();
                    List<ProfileTypeModel> typeList = new List<ProfileTypeModel>();
                    List<ProfileGroupModel> groupList = new List<ProfileGroupModel>();
                    //List<ExistProfileModel> existList = new List<ExistProfileModel>();

                    foreach (DataRow item in dataTable.Rows)
                    {
                        if(item["KUNNR"].ToString() == "12005051")
                        {

                        }
                        try
                        {
                            //Mã SAP
                            var ProfileForeignCode = item["KUNNR"].ToString();
                            //Mã CRM
                            var ProfileCode_Str = item["LOCCO"].ToString();
                            int ProfileCode = 0;
                            if (!string.IsNullOrEmpty(ProfileCode_Str))
                            {
                                ProfileCode = Convert.ToInt32(item["LOCCO"].ToString());
                            }
                            //Mã công ty
                            var CompanyCode = item["BUKRS"].ToString();
                            //Mã SAP lớn hơn 8 ký tự là loại địa chỉ
                            bool isAddressBook = false;
                            if (ProfileForeignCode.Length > 8)
                            {
                                isAddressBook = true;
                            }
                            //Tạo mới ProfileId
                            var ProfileId = Guid.NewGuid();
                            //DateTime
                            UtilitiesRepository _utilities = new UtilitiesRepository();
                            var CreateTime = _utilities.ConvertDateTime(item["ERDAT"].ToString());
                            var Phone = item["TEL_NUMBER"].ToString();
                            var TaxNo = item["STCEG"].ToString();
                            //Trong nước | Nước ngoài: 
                            //1. Nếu LAND1 == "VN" => trong nước
                            //2. Nếu LAND1 != "VN" => nước ngoài
                            bool isForeignCustomer = false;
                            string CountryCode = item["LAND1"].ToString();
                            if (!string.IsNullOrEmpty(CountryCode) && CountryCode != "VN")
                            {
                                isForeignCustomer = true;
                            }
                            //Tỉnh thành
                            var ProvinceCode = item["VKGRP"].ToString();
                            Guid? ProvinceId = null;
                            //Nếu là khách nước ngoài => lưu province là quốc gia
                            if (isForeignCustomer == true && !string.IsNullOrEmpty(CountryCode))
                            {
                                ProvinceId = provinceLst.Where(p => p.ProvinceCode == CountryCode)
                                                .Select(p => p.ProvinceId).FirstOrDefault();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ProvinceCode))
                                {
                                    ProvinceId = provinceLst.Where(p => p.ProvinceCode == ProvinceCode)
                                                            .Select(p => p.ProvinceId).FirstOrDefault();
                                }
                                else
                                {
                                    //Nếu không có Tỉnh thành => Lưu "Khác"
                                    ProvinceId = Guid.Parse("33b02d3c-c3dc-41ca-a909-09c951b9ad22");
                                }
                            }

                            //Quận huyện
                            //var DistrictCode = item["BZIRK"].ToString();
                            //Guid? DistrictId = null;
                            //if (!string.IsNullOrEmpty(DistrictCode) && DistrictCode != "999999")
                            //{
                            //    DistrictId = districtLst.Where(p => p.DistrictCode == DistrictCode)
                            //                            .Select(p => p.DistrictId).FirstOrDefault();
                            //}
                            var DistrictName = item["BAHNE"].ToString();
                            Guid? DistrictId = null;
                            if (!string.IsNullOrEmpty(DistrictName) && ProvinceId.HasValue)
                            {
                                if (DistrictName == "Phan Rang - Tháp Chàm")
                                {
                                    DistrictName = "Thành Phố " + DistrictName;
                                }
                                var district = districtLst.Where(p => DistrictName == (p.Appellation + " " + p.DistrictName) && p.ProvinceId == ProvinceId).FirstOrDefault();
                                if (district != null)
                                {
                                    DistrictId = district.DistrictId;
                                }
                            }
                            //Phường xã
                            //var WardName = item["BAHNS_BAHNE"].ToString();
                            var WardName = item["BAHNS"].ToString();
                            Guid? WardId = null;
                            if (!string.IsNullOrEmpty(WardName) && DistrictId.HasValue)
                            {
                                var ward = wardLst.Where(p => WardName == (p.Appellation + " " + p.WardName) && p.DistrictId == DistrictId).FirstOrDefault();
                                if (ward != null)
                                {
                                    WardId = ward.WardId;
                                }
                            }
                            //Phân loại khách hàng: 1.KH Tiêu dùng. 2.KH Doanh nghiệp
                            var PhanLoaiKH = item["NHOMKH"].ToString();
                            var ProfileTypeCode = string.Empty;
                            if (!string.IsNullOrEmpty(PhanLoaiKH))
                            {
                                if (PhanLoaiKH == "1")
                                {
                                    ProfileTypeCode = ConstCustomerType.Customer;
                                }
                                else if (PhanLoaiKH == "2")
                                {
                                    ProfileTypeCode = ConstCustomerType.Bussiness;
                                }
                            }

                            //Email
                            string Email = item["SMTP_ADDR"].ToString();

                            #region Profile
                            //Check profile exists có mã CRM
                            var existProfileWithCRMCode = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileCode == ProfileCode).FirstOrDefault();
                            //Nếu đã có profile trong DB và chưa có Mã SAP => cập nhật
                            if (existProfileWithCRMCode != null
                                //Chưa có mã SAP thì cập nhật
                                //&& string.IsNullOrEmpty(existProfileWithCRMCode.ProfileForeignCode)
                                )
                            {
                                if (isAddressBook == false)
                                {
                                    //Nếu mã SAP đã tồn tại => check mã CRM
                                    // => Trùng mã CRM thì cập nhật
                                    // => Khác mã CRM thì báo cho user
                                    var existsSAPProfile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).Select(p => new
                                    {
                                        p.ProfileCode,
                                        p.ProfileForeignCode
                                    }).FirstOrDefault();
                                    //CRM chưa có mã SAP hoặc trùng mã CRM thì mới cập nhật
                                    if (existsSAPProfile == null || existsSAPProfile.ProfileCode == ProfileCode)
                                    {
                                        //1. GUID
                                        ProfileId = existProfileWithCRMCode.ProfileId;
                                        //2. ProfileCode
                                        //2. ProfileCode => cannot update identity column
                                        //if (ProfileCode != 0)
                                        //{
                                        //    existProfileWithCRMCode.ProfileCode = ProfileCode;
                                        //}
                                        //3. ProfileForeignCode
                                        existProfileWithCRMCode.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        existProfileWithCRMCode.isForeignCustomer = isForeignCustomer;
                                        existProfileWithCRMCode.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        existProfileWithCRMCode.Title = Title;
                                        ////6. Loại
                                        existProfileWithCRMCode.CustomerTypeCode = ConstCustomerType.Account;

                                        //Phân loại KH
                                        var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                        if (existProfileType == null)
                                        {
                                            ProfileTypeModel profileType = new ProfileTypeModel();
                                            profileType.ProfileTypeId = Guid.NewGuid();
                                            profileType.ProfileId = ProfileId;
                                            //if (existProfileWithCRMCode.Title == "Company")
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                            //}
                                            //else
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                            //}
                                            profileType.CustomerTypeCode = ProfileTypeCode;
                                            profileType.CompanyCode = item["BUKRS"].ToString();
                                            profileType.CreateBy = SYSTEM;
                                            profileType.CreateTime = CreateTime;
                                            _context.Entry(profileType).State = EntityState.Added;
                                            //typeList.Add(profileType);
                                        }
                                        else
                                        {
                                            existProfileType.CustomerTypeCode = ProfileTypeCode;
                                            _context.Entry(existProfileType).State = EntityState.Modified;
                                        }
                                        //7. Họ va Tên|Tên công ty
                                        existProfileWithCRMCode.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        existProfileWithCRMCode.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        existProfileWithCRMCode.AbbreviatedName = existProfileWithCRMCode.ProfileName?.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        /*
                                        try
                                        {
                                            //Delete all
                                            var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                            if (phoneExistLst != null && phoneExistLst.Count > 0)
                                            {
                                                _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                            }
                                            //Add again
                                            if (!string.IsNullOrEmpty(Phone))
                                            {
                                                if (Phone.Contains("-"))
                                                {
                                                    var arr = Phone.Split('-').ToList();
                                                    var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                                    var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                    foreach (var phoneItem in phoneArray)
                                                    {
                                                        ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                        phoneModel.PhoneId = Guid.NewGuid();
                                                        phoneModel.ProfileId = ProfileId;
                                                        phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                        if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                        {
                                                            _context.Entry(phoneModel).State = EntityState.Added;
                                                            //phoneList.Add(phoneModel);
                                                        }
                                                    }
                                                    existProfileWithCRMCode.Phone = PhoneNumber.Trim();
                                                }
                                                else
                                                {
                                                    existProfileWithCRMCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                                }
                                            }
                                            else
                                            {
                                                existProfileWithCRMCode.Phone = null;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // ghi log
                                            string mess = ex.Message;
                                            if (ex.InnerException != null)
                                            {
                                                mess = ex.InnerException.Message;
                                                if (ex.InnerException.InnerException != null)
                                                {
                                                    mess = ex.InnerException.InnerException.Message;
                                                }
                                            }
                                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                            continue;
                                        }
                                        */
                                        //15. Email
                                        //existProfileWithCRMCode.Email = item["SMTP_ADDR"].ToString();
                                        //if (!string.IsNullOrEmpty(existProfileWithCRMCode.Email))
                                        //{
                                        //    if (IsValidEmail(existProfileWithCRMCode.Email) == false)
                                        //    {
                                        //        existProfileWithCRMCode.Note = string.Format("(Email: {0})", existProfileWithCRMCode.Email);
                                        //        existProfileWithCRMCode.Email = null;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!string.IsNullOrEmpty(existProfileWithCRMCode.Note) && existProfileWithCRMCode.Note.Contains("Email:"))
                                        //        {
                                        //            existProfileWithCRMCode.Note = null;
                                        //        }
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    existProfileWithCRMCode.Email = null;
                                        //}
                                        /*
                                        if (!string.IsNullOrEmpty(Email))
                                        {
                                            try
                                            {
                                                //Delete all
                                                var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                if (emailExistLst != null && emailExistLst.Count > 0)
                                                {
                                                    _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                                }
                                                //Add again
                                                if (Email.Contains(";"))
                                                {
                                                    var emailArray = Email.Split(';').ToList();
                                                    foreach (var emailItem in emailArray)
                                                    {
                                                        if (IsValidEmail(emailItem) == true)
                                                        {
                                                            ProfileEmailModel emailModel = new ProfileEmailModel();
                                                            emailModel.EmailId = Guid.NewGuid();
                                                            emailModel.ProfileId = ProfileId;
                                                            emailModel.Email = emailItem.Trim();
                                                            _context.Entry(emailModel).State = EntityState.Added;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsValidEmail(Email) == true)
                                                    {
                                                        ProfileEmailModel emailModel = new ProfileEmailModel();
                                                        emailModel.EmailId = Guid.NewGuid();
                                                        emailModel.ProfileId = ProfileId;
                                                        emailModel.Email = Email.Trim();
                                                        _context.Entry(emailModel).State = EntityState.Added;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // ghi log
                                                string mess = ex.Message;
                                                if (ex.InnerException != null)
                                                {
                                                    mess = ex.InnerException.Message;
                                                    if (ex.InnerException.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.InnerException.Message;
                                                    }
                                                }
                                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                continue;
                                            }
                                        }
                                        */
                                        //16. Khu vực
                                        //existProfileWithCRMCode.SaleOfficeCode = item["VKBUR"].ToString();
                                        //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                        var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(SaleOfficeCode))
                                        {
                                            existProfileWithCRMCode.SaleOfficeCode = SaleOfficeCode;
                                        }
                                        //17. Địa chỉ
                                        //existProfileWithCRMCode.Address = item["ADDRESS"].ToString();
                                        existProfileWithCRMCode.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString().FirstCharToUpper() : item["ADDRESS"].ToString().FirstCharToUpper();
                                        //18. Tỉnh/Thành phố
                                        existProfileWithCRMCode.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        existProfileWithCRMCode.DistrictId = DistrictId;
                                        //20. Phường/22. 
                                        existProfileWithCRMCode.WardId = WardId;
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái (not update this field)
                                        //Trạng thái hoạt động: X: ngưng hoạt động
                                        string TrangThaiHoatDong = item["LOEVM"].ToString();
                                        bool isActived = true;
                                        if (!string.IsNullOrEmpty(TrangThaiHoatDong) && TrangThaiHoatDong.ToUpper() == "X")
                                        {
                                            isActived = false;
                                        }
                                        if (existProfileWithCRMCode.Actived != isActived)
                                        {
                                            existProfileWithCRMCode.Actived = isActived;
                                        }
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty 
                                        existProfileWithCRMCode.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        existProfileWithCRMCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy (not update this field)
                                        //29. Thời gian tạo
                                        //exist.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        existProfileWithCRMCode.LastEditBy = SYSTEM;
                                        //31. Thời gian sửa
                                        existProfileWithCRMCode.LastEditTime = DateTime.Now;
                                        //32. Phân nhóm KH/ Customer Account Group
                                        existProfileWithCRMCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        existProfileWithCRMCode.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        existProfileWithCRMCode.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        existProfileWithCRMCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        existProfileWithCRMCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        existProfileWithCRMCode.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                       // existProfileWithCRMCode.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            existProfileWithCRMCode.TaxNo = TaxNo;
                                        }
                                        //nguồn KH
                                        existProfileWithCRMCode.CustomerSourceCode = "SHOWROOM";
                                        //Loại địa chỉ
                                        string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                        if (!string.IsNullOrEmpty(LoaiDiaChi))
                                        {
                                            existProfileWithCRMCode.AddressTypeCode = LoaiDiaChi;
                                        }
                                        //Ngành nghề
                                        string NganhNghe = item["BRSCH"].ToString();
                                        if (!string.IsNullOrEmpty(NganhNghe))
                                        {
                                            //NganhNghe = NganhNghe.PadLeft(4, '0');

                                            var existProfileCareer = _context.ProfileCareerModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                            if (existProfileCareer == null)
                                            {
                                                ProfileCareerModel profileCareer = new ProfileCareerModel();
                                                profileCareer.ProfileCareerId = Guid.NewGuid();
                                                profileCareer.ProfileId = ProfileId;
                                                profileCareer.ProfileCareerCode = NganhNghe;
                                                profileCareer.CompanyCode = item["BUKRS"].ToString();
                                                profileCareer.CreateBy = SYSTEM;
                                                profileCareer.CreateTime = CreateTime;
                                                _context.Entry(profileCareer).State = EntityState.Added;
                                            }
                                            else
                                            {
                                                existProfileCareer.ProfileCareerCode = NganhNghe;
                                                _context.Entry(existProfileCareer).State = EntityState.Modified;
                                            }
                                        }
                                        //Yêu cầu tạo khách ở ECC
                                        existProfileWithCRMCode.isCreateRequest = false;
                                        _context.Entry(existProfileWithCRMCode).State = EntityState.Modified;

                                        //thêm nhóm KH
                                        AddProfileGroup(item, CompanyCode, ProfileId);

                                        //thêm NVKD
                                        AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);
                                    }
                                    else
                                    {
                                        WriteLogFile(logFilePath, "Sync Profile error: Update CRM: " + ProfileCode + " - Voi SAP: " + ProfileForeignCode + ". Nhung da Ton tai:" + existsSAPProfile.ProfileCode);
                                    }
                                    //profileCodeList.Add(ProfileForeignCode);
                                }
                                //cập nhật address book
                                else
                                {

                                }
                            }

                            //Nếu không phải là yêu cầu cập nhật => tìm theo mã SAP
                            else if (existProfileWithCRMCode == null)
                            {
                                //Tìm theo mã SAP
                                var existProfileWithSAPCode = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                if (existProfileWithSAPCode == null)
                                {
                                    //Nếu phân nhóm KH là Z012 thì không thêm vào DB
                                    var PNKH = item["KTOKD"].ToString();
                                    if (isAddressBook == false && (string.IsNullOrEmpty(PNKH) || PNKH != "Z012"))
                                    {
                                        ProfileModel model = new ProfileModel();
                                        //1. GUID
                                        model.ProfileId = ProfileId;
                                        //2. ProfileCode
                                        if (ProfileCode != 0)
                                        {
                                            model.ProfileCode = ProfileCode;
                                        }
                                        //3. ProfileForeignCode
                                        model.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        model.isForeignCustomer = isForeignCustomer;
                                        model.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        model.Title = Title;
                                        //6. Loại
                                        model.CustomerTypeCode = ConstCustomerType.Account;

                                        #region  //Phân loại KH
                                        ProfileTypeModel profileType = new ProfileTypeModel();
                                        profileType.ProfileTypeId = Guid.NewGuid();
                                        profileType.ProfileId = ProfileId;
                                        //if (model.Title == "Company")
                                        //{
                                        //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                        //}
                                        //else
                                        //{
                                        //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                        //}
                                        profileType.CustomerTypeCode = ProfileTypeCode;
                                        profileType.CompanyCode = item["BUKRS"].ToString();
                                        profileType.CreateBy = SYSTEM;
                                        profileType.CreateTime = CreateTime;
                                        _context.Entry(profileType).State = EntityState.Added;
                                        #endregion
                                        //typeList.Add(profileType);
                                        //7. Họ va Tên|Tên công ty
                                        model.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        model.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        model.AbbreviatedName = model.ProfileName.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Phone))
                                            {
                                                if (Phone.Contains("-"))
                                                {
                                                    var arr = Phone.Split('-').ToList();
                                                    var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);
                                                    var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                    foreach (var phoneItem in phoneArray)
                                                    {
                                                        ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                        phoneModel.PhoneId = Guid.NewGuid();
                                                        phoneModel.ProfileId = ProfileId;
                                                        phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                        if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                        {
                                                            _context.Entry(phoneModel).State = EntityState.Added;
                                                            //phoneList.Add(phoneModel);
                                                        }
                                                    }
                                                    model.Phone = PhoneNumber.Trim();
                                                }
                                                else
                                                {
                                                    model.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone).Trim();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // ghi log
                                            string mess = ex.Message;
                                            if (ex.InnerException != null)
                                            {
                                                mess = ex.InnerException.Message;
                                                if (ex.InnerException.InnerException != null)
                                                {
                                                    mess = ex.InnerException.InnerException.Message;
                                                }
                                            }
                                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                            continue;
                                        }
                                        //15. Email
                                        //model.Email = item["SMTP_ADDR"].ToString();
                                        //if (!string.IsNullOrEmpty(model.Email) && IsValidEmail(model.Email) == false)
                                        //{
                                        //    model.Note = string.Format("(Email: {0})", model.Email);
                                        //    model.Email = null;
                                        //}
                                        //else
                                        //{
                                        //    model.Email = null;
                                        //}
                                        if (!string.IsNullOrEmpty(Email))
                                        {
                                            try
                                            {
                                                //Delete all
                                                var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                if (emailExistLst != null && emailExistLst.Count > 0)
                                                {
                                                    _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                                }
                                                //Add again
                                                if (Email.Contains(";"))
                                                {
                                                    var emailArray = Email.Split(';').ToList();
                                                    foreach (var emailItem in emailArray)
                                                    {
                                                        if (IsValidEmail(emailItem) == true)
                                                        {
                                                            ProfileEmailModel emailModel = new ProfileEmailModel();
                                                            emailModel.EmailId = Guid.NewGuid();
                                                            emailModel.ProfileId = ProfileId;
                                                            emailModel.Email = emailItem.Trim();
                                                            _context.Entry(emailModel).State = EntityState.Added;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsValidEmail(Email) == true)
                                                    {
                                                        ProfileEmailModel emailModel = new ProfileEmailModel();
                                                        emailModel.EmailId = Guid.NewGuid();
                                                        emailModel.ProfileId = ProfileId;
                                                        emailModel.Email = Email.Trim();
                                                        _context.Entry(emailModel).State = EntityState.Added;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // ghi log
                                                string mess = ex.Message;
                                                if (ex.InnerException != null)
                                                {
                                                    mess = ex.InnerException.Message;
                                                    if (ex.InnerException.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.InnerException.Message;
                                                    }
                                                }
                                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                continue;
                                            }
                                        }
                                        //16. Khu vực
                                        //model.SaleOfficeCode = item["VKBUR"].ToString();
                                        //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                        var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(SaleOfficeCode))
                                        {
                                            model.SaleOfficeCode = SaleOfficeCode;
                                        }
                                        //17. Địa chỉ
                                        //model.Address = item["ADDRESS"].ToString();
                                        model.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString().FirstCharToUpper() : item["ADDRESS"].ToString().FirstCharToUpper();
                                        //18. Tỉnh/Thành phố
                                        model.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        model.DistrictId = DistrictId;
                                        //20. Phường/Xã
                                        model.WardId = WardId;
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái
                                        //model.Actived = true;
                                        //Trạng thái hoạt động: X: ngưng hoạt động
                                        string TrangThaiHoatDong = item["LOEVM"].ToString();
                                        bool isActived = true;
                                        if (!string.IsNullOrEmpty(TrangThaiHoatDong) && TrangThaiHoatDong.ToUpper() == "X")
                                        {
                                            isActived = false;
                                        }
                                        model.Actived = isActived;
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty
                                        model.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        model.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy
                                        model.CreateBy = SYSTEM;
                                        //29. Thời gian tạo
                                        model.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        //31. Thời gian tạo
                                        //32. Phân nhóm KH/ Customer Account Group
                                        model.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        model.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        model.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        model.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        model.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        model.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                        model.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            model.TaxNo = TaxNo;
                                        }
                                        //nguồn KH
                                        model.CustomerSourceCode = "SHOWROOM";
                                        //Loại địa chỉ
                                        string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                        if (!string.IsNullOrEmpty(LoaiDiaChi))
                                        {
                                            model.AddressTypeCode = LoaiDiaChi;
                                        }
                                        //Ngành nghề
                                        string NganhNghe = item["BRSCH"].ToString();
                                        if (!string.IsNullOrEmpty(NganhNghe))
                                        {
                                            //NganhNghe = NganhNghe.PadLeft(4, '0');

                                            ProfileCareerModel profileCareer = new ProfileCareerModel();
                                            profileCareer.ProfileCareerId = Guid.NewGuid();
                                            profileCareer.ProfileId = ProfileId;
                                            profileCareer.ProfileCareerCode = NganhNghe;
                                            profileCareer.CompanyCode = item["BUKRS"].ToString();
                                            profileCareer.CreateBy = SYSTEM;
                                            profileCareer.CreateTime = CreateTime;
                                            _context.Entry(profileCareer).State = EntityState.Added;

                                        }
                                        //Yêu cầu tạo khách ở ECC
                                        model.isCreateRequest = false;
                                        //model.CreateRequestTime = DateTime.Now;

                                        _context.Entry(model).State = EntityState.Added;
                                        //thêm nhóm KH
                                        AddProfileGroup(item, CompanyCode, ProfileId);

                                        //thêm NVKD
                                        AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);

                                        //profileCodeList.Add(ProfileForeignCode);
                                        //if (profileCodeList.Contains(ProfileForeignCode))
                                        //{
                                        //    profileList.Add(model);
                                        //}
                                    }
                                    //thêm address book
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    if (isAddressBook == false)
                                    {
                                        //1. GUID
                                        ProfileId = existProfileWithSAPCode.ProfileId;
                                        //2. ProfileCode => cannot update ProfileCode => identity
                                        //if (ProfileCode != 0)
                                        //{
                                        //    existProfileWithSAPCode.ProfileCode = ProfileCode;
                                        //}
                                        //3. ProfileForeignCode
                                        existProfileWithSAPCode.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        existProfileWithSAPCode.isForeignCustomer = isForeignCustomer;
                                        existProfileWithSAPCode.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        existProfileWithSAPCode.Title = Title;
                                        ////6. Loại
                                        existProfileWithSAPCode.CustomerTypeCode = ConstCustomerType.Account;

                                        //Phân loại KH
                                        var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                        if (existProfileType == null)
                                        {
                                            ProfileTypeModel profileType = new ProfileTypeModel();
                                            profileType.ProfileTypeId = Guid.NewGuid();
                                            profileType.ProfileId = ProfileId;
                                            //if (existProfileWithSAPCode.Title == "Company")
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                            //}
                                            //else
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                            //}
                                            profileType.CustomerTypeCode = ProfileTypeCode;
                                            profileType.CompanyCode = item["BUKRS"].ToString();
                                            profileType.CreateBy = SYSTEM;
                                            profileType.CreateTime = CreateTime;
                                            _context.Entry(profileType).State = EntityState.Added;
                                            //typeList.Add(profileType);
                                        }
                                        else
                                        {
                                            existProfileType.CustomerTypeCode = ProfileTypeCode;
                                            _context.Entry(existProfileType).State = EntityState.Modified;
                                        }
                                        //7. Họ va Tên|Tên công ty
                                        existProfileWithSAPCode.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        existProfileWithSAPCode.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        existProfileWithSAPCode.AbbreviatedName = existProfileWithSAPCode.ProfileName.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        
                                        //try
                                        //{
                                        //    //Delete all
                                        //    var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                        //    if (phoneExistLst != null && phoneExistLst.Count > 0)
                                        //    {
                                        //        _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                        //    }
                                        //    //Add again
                                        //    if (!string.IsNullOrEmpty(Phone))
                                        //    {
                                        //        if (Phone.Contains("-"))
                                        //        {
                                        //            var arr = Phone.Split('-').ToList();
                                        //            var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                        //            var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                        //            foreach (var phoneItem in phoneArray)
                                        //            {
                                        //                ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                        //                phoneModel.PhoneId = Guid.NewGuid();
                                        //                phoneModel.ProfileId = ProfileId;
                                        //                phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                        //                if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                        //                {
                                        //                    _context.Entry(phoneModel).State = EntityState.Added;
                                        //                    //phoneList.Add(phoneModel);
                                        //                }
                                        //            }
                                        //            existProfileWithSAPCode.Phone = PhoneNumber.Trim();
                                        //        }
                                        //        else
                                        //        {
                                        //            existProfileWithSAPCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        existProfileWithSAPCode.Phone = null;
                                        //    }
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    // ghi log
                                        //    string mess = ex.Message;
                                        //    if (ex.InnerException != null)
                                        //    {
                                        //        mess = ex.InnerException.Message;
                                        //        if (ex.InnerException.InnerException != null)
                                        //        {
                                        //            mess = ex.InnerException.InnerException.Message;
                                        //        }
                                        //    }
                                        //    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                        //    continue;
                                        //}
                                        
                                        //15. Email
                                        //existProfileWithSAPCode.Email = item["SMTP_ADDR"].ToString();
                                        //if (!string.IsNullOrEmpty(existProfileWithSAPCode.Email))
                                        //{
                                        //    if (IsValidEmail(existProfileWithSAPCode.Email) == false)
                                        //    {
                                        //        existProfileWithSAPCode.Note = string.Format("(Email: {0})", existProfileWithSAPCode.Email);
                                        //        existProfileWithSAPCode.Email = null;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!string.IsNullOrEmpty(existProfileWithSAPCode.Note) && existProfileWithSAPCode.Note.Contains("Email:"))
                                        //        {
                                        //            existProfileWithSAPCode.Note = null;
                                        //        }
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    existProfileWithSAPCode.Email = null;
                                        //}
                                        //if (!string.IsNullOrEmpty(Email))
                                        //{
                                        //    try
                                        //    {
                                        //        //Delete all
                                        //        var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                        //        if (emailExistLst != null && emailExistLst.Count > 0)
                                        //        {
                                        //            _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                        //        }
                                        //        //Add again
                                        //        if (Email.Contains(";"))
                                        //        {
                                        //            var emailArray = Email.Split(';').ToList();
                                        //            foreach (var emailItem in emailArray)
                                        //            {
                                        //                if (IsValidEmail(emailItem) == true)
                                        //                {
                                        //                    ProfileEmailModel emailModel = new ProfileEmailModel();
                                        //                    emailModel.EmailId = Guid.NewGuid();
                                        //                    emailModel.ProfileId = ProfileId;
                                        //                    emailModel.Email = emailItem.Trim();
                                        //                    _context.Entry(emailModel).State = EntityState.Added;
                                        //                }
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            if (IsValidEmail(Email) == true)
                                        //            {
                                        //                ProfileEmailModel emailModel = new ProfileEmailModel();
                                        //                emailModel.EmailId = Guid.NewGuid();
                                        //                emailModel.ProfileId = ProfileId;
                                        //                emailModel.Email = Email.Trim();
                                        //                _context.Entry(emailModel).State = EntityState.Added;
                                        //            }
                                        //        }
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        // ghi log
                                        //        string mess = ex.Message;
                                        //        if (ex.InnerException != null)
                                        //        {
                                        //            mess = ex.InnerException.Message;
                                        //            if (ex.InnerException.InnerException != null)
                                        //            {
                                        //                mess = ex.InnerException.InnerException.Message;
                                        //            }
                                        //        }
                                        //        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                        //        continue;
                                        //    }
                                        //}
                                        //16. Khu vực
                                        //existProfileWithSAPCode.SaleOfficeCode = item["VKBUR"].ToString();
                                        //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                        var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(SaleOfficeCode))
                                        {
                                            existProfileWithSAPCode.SaleOfficeCode = SaleOfficeCode;
                                        }
                                        //17. Địa chỉ
                                        //existProfileWithSAPCode.Address = item["ADDRESS"].ToString();
                                        existProfileWithSAPCode.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString().FirstCharToUpper() : item["ADDRESS"].ToString().FirstCharToUpper();
                                        //18. Tỉnh/Thành phố
                                        existProfileWithSAPCode.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        existProfileWithSAPCode.DistrictId = DistrictId;
                                        //20. Phường/Xã
                                        existProfileWithSAPCode.WardId = WardId;
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái (not update this field)
                                        //Trạng thái hoạt động: X: ngưng hoạt động
                                        string TrangThaiHoatDong = item["LOEVM"].ToString();
                                        bool isActived = true;
                                        if (!string.IsNullOrEmpty(TrangThaiHoatDong) && TrangThaiHoatDong.ToUpper() == "X")
                                        {
                                            isActived = false;
                                        }
                                        if (existProfileWithSAPCode.Actived != isActived)
                                        {
                                            existProfileWithSAPCode.Actived = isActived;
                                        }
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty 
                                        existProfileWithSAPCode.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        existProfileWithSAPCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy (not update this field)
                                        //29. Thời gian tạo
                                        //exist.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        existProfileWithSAPCode.LastEditBy = SYSTEM;
                                        //31. Thời gian sửa
                                        existProfileWithSAPCode.LastEditTime = DateTime.Now;
                                        //32. Phân nhóm KH/ Customer Account Group
                                        existProfileWithSAPCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        existProfileWithSAPCode.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        existProfileWithSAPCode.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        existProfileWithSAPCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        existProfileWithSAPCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        existProfileWithSAPCode.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                       // existProfileWithSAPCode.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            existProfileWithSAPCode.TaxNo = TaxNo;
                                        }
                                        existProfileWithSAPCode.CustomerSourceCode = "SHOWROOM";
                                        //Loại địa chỉ
                                        string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                        if (!string.IsNullOrEmpty(LoaiDiaChi))
                                        {
                                            existProfileWithSAPCode.AddressTypeCode = LoaiDiaChi;
                                        }
                                        //Ngành nghề
                                        string NganhNghe = item["BRSCH"].ToString();
                                        if (!string.IsNullOrEmpty(NganhNghe))
                                        {
                                            //NganhNghe = NganhNghe.PadLeft(4, '0');

                                            var existProfileCareer = _context.ProfileCareerModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                            if (existProfileCareer == null)
                                            {
                                                ProfileCareerModel profileCareer = new ProfileCareerModel();
                                                profileCareer.ProfileCareerId = Guid.NewGuid();
                                                profileCareer.ProfileId = ProfileId;
                                                profileCareer.ProfileCareerCode = NganhNghe;
                                                profileCareer.CompanyCode = item["BUKRS"].ToString();
                                                profileCareer.CreateBy = SYSTEM;
                                                profileCareer.CreateTime = CreateTime;
                                                _context.Entry(profileCareer).State = EntityState.Added;
                                            }
                                            else
                                            {
                                                existProfileCareer.ProfileCareerCode = NganhNghe;
                                                _context.Entry(existProfileCareer).State = EntityState.Modified;
                                            }
                                        }

                                        //Yêu cầu tạo khách ở ECC
                                        existProfileWithSAPCode.isCreateRequest = false;
                                        _context.Entry(existProfileWithSAPCode).State = EntityState.Modified;

                                        //thêm nhóm KH
                                        AddProfileGroup(item, CompanyCode, ProfileId);

                                        //thêm NVKD
                                        AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);
                                        //profileCodeList.Add(ProfileForeignCode);
                                    }
                                    //cập nhật address book
                                    else
                                    {

                                    }
                                }
                            }
                            #endregion

                            #region ProfileBAttributeModel: TaxNo
                            //Delete all
                            //var existAttr = _context.ProfileBAttributeModel
                            //                        .FirstOrDefault(p => p.ProfileId == ProfileId);
                            //if (existAttr != null)
                            //{
                            //    _context.Entry(existAttr).State = EntityState.Deleted;
                            //}
                            ////Add again
                            //ProfileBAttributeModel attr = new ProfileBAttributeModel();
                            //attr.ProfileId = ProfileId;
                            //if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                            //{
                            //    attr.TaxNo = TaxNo;
                            //}
                            //_context.Entry(attr).State = EntityState.Added;
                            //if (!profileCodeList.Contains(ProfileForeignCode))
                            //{
                            //    bAttrList.Add(attr);
                            //}
                            #endregion

                            #region RoleInChargeModel
                            //var roleCharge = _context.RoleInChargeModel
                            //                         .FirstOrDefault(p => p.ProfileId == ProfileId && p.CreateBy == SYSTEM);
                            //if (roleCharge != null)
                            //{
                            //    _context.Entry(roleCharge).State = EntityState.Deleted;
                            //}

                            //var RolesCode = item["ZP_DEPT"].ToString();
                            //if (!string.IsNullOrEmpty(RolesCode) && RolesCode != "00000000")
                            //{
                            //    var role = _context.RolesModel.Where(p => p.RolesCode == RolesCode).FirstOrDefault();
                            //    if (role != null)
                            //    {
                            //        RoleInChargeModel roleModel = new RoleInChargeModel();
                            //        roleModel.RoleInChargeId = Guid.NewGuid();
                            //        roleModel.ProfileId = ProfileId;
                            //        roleModel.RolesId = role.RolesId;
                            //        roleModel.CreateBy = SYSTEM;
                            //        roleModel.CreateTime = CreateTime;
                            //        _context.Entry(roleModel).State = EntityState.Added;
                            //        //if (profileCodeList.Contains(ProfileForeignCode))
                            //        //{
                            //        //    roleList.Add(roleModel);
                            //        //}
                            //    }
                            //}
                            #endregion

                            #region AddressBookModel
                            //if (isContact == true)
                            //{
                            //    var existAddress = _context.AddressBookModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                            //    if (existAddress != null)
                            //    {
                            //        _context.Entry(existAddress).State = EntityState.Deleted;
                            //    }

                            //    AddressBookModel address = new AddressBookModel();
                            //    address.AddressBookId = Guid.NewGuid();
                            //    address.ProfileId = ProfileId;
                            //    address.Address = item["ADDRESS"].ToString();
                            //    address.ProvinceId = ProvinceId;
                            //    address.DistrictId = DistrictId;
                            //    address.CountryCode = ConstCountry.VN;
                            //    address.AddressTypeCode = ConstAddressType.GH;
                            //    address.CreateBy = SYSTEM;
                            //    address.CreateTime = DateTime.Now;
                            //    //_context.Entry(address).State = EntityState.Added;
                            //    addressList.Add(address);
                            //}
                            #endregion

                            #region ProfileContactAttributeModel
                            //if (isContact == true)
                            //{
                            //    var existProfileCode = ProfileForeignCode.Substring(0, 8);
                            //    var existProfile = _context.ProfileModel.FirstOrDefault(p => p.ProfileForeignCode == existProfileCode);
                            //    if (existProfile != null)
                            //    {
                            //        var existContact = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                            //        if (existContact != null)
                            //        {
                            //            _context.Entry(existContact).State = EntityState.Deleted;
                            //        }

                            //        ProfileContactAttributeModel contactAttr = new ProfileContactAttributeModel();
                            //        contactAttr.ProfileId = ProfileId;
                            //        contactAttr.CompanyId = existProfile.ProfileId;
                            //        //_context.Entry(contactAttr).State = EntityState.Added;
                            //        contactList.Add(contactAttr);
                            //    }
                            //}
                            #endregion

                            _context.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors (ProfileSAPCode: {2}): ",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State, item["KUNNR"].ToString()));
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    if (ve.PropertyName == "Email")
                                    {
                                        //var ProfileForeignCode = item["KUNNR"].ToString();
                                        //var profile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                        //if (profile != null && !string.IsNullOrEmpty(profile.Email))
                                        //{
                                        //    profile.Note = string.Format("(Email: {0})", profile.Email);
                                        //    profile.Email = null;
                                        //    _context.Entry(profile).State = EntityState.Modified;
                                        //    _context.SaveChanges();

                                        //    WriteLogFile(logFilePath, "Update Email => Note");
                                        //}
                                        WriteLogFile(logFilePath, "Email: " + item["SMTP_ADDR"].ToString());
                                    }
                                    else
                                    {
                                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                    }
                                }
                            }
                            continue;
                        }
                        catch (Exception ex)
                        {
                            var mess = ex.Message;
                            if (ex.InnerException != null)
                            {
                                if (ex.InnerException.InnerException != null)
                                {
                                    mess = ex.InnerException.InnerException.Message;
                                }
                                else
                                {
                                    mess = ex.InnerException.Message;
                                }
                            }
                            WriteLogFile(logFilePath, "Sync Data error: " + mess + " SAPCode: " + item["KUNNR"].ToString());
                            continue;
                        }
                    }
                    //if (profileList != null && profileList.Count > 0)
                    //{
                    //    _context.ProfileModel.AddRange(profileList);
                    //}
                    //_context.ProfilePhoneModel.AddRange(phoneList);
                    //_context.ProfileBAttributeModel.AddRange(bAttrList);
                    //_context.PersonInChargeModel.AddRange(personList);
                    //_context.RoleInChargeModel.AddRange(roleList);
                    //_context.ProfileContactAttributeModel.AddRange(contactList);
                    //_context.ProfileTypeModel.AddRange(typeList);
                    //_context.ProfileGroupModel.AddRange(groupList);

                    //_context.ChangeTracker.DetectChanges();
                    //_context.SaveChanges();

                    //MessageBox.Show("Đã đồng bộ thành công!");

                    WriteLogFile(logFilePath, string.Format("SYNC PROFILE: Stop. (Company: {0})", Company));
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var mess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            mess = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            mess = ex.InnerException.Message;
                        }
                    }
                    //ghi log
                    //MessageBox.Show(mess);
                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                }
                //}
                //MessageBox.Show("Đã đồng bộ: " + dt.Rows.Count + " dòng.");
            }
        }

        private void AddPersonInCharge(List<string> employeeList, DataRow item, string CompanyCode, Guid ProfileId, DateTime? CreateTime)
        {
            #region PersonInChargeModel
            var existPerson = _context.PersonInChargeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).ToList();
            //Nếu có tồn tại thì xóa add lại
            if (existPerson != null && existPerson.Count > 0)
            {
                _context.PersonInChargeModel.RemoveRange(existPerson);
                _context.SaveChanges();
            }

            //if (existPerson == null || existPerson.Count == 0)
            //{
            //Sale Employee
            var EmployeeCode = item["EMPLOYEENO"].ToString();
            if (!string.IsNullOrEmpty(EmployeeCode) && EmployeeCode != "00000000" && employeeList.Contains(EmployeeCode))
            {
                PersonInChargeModel person = new PersonInChargeModel();
                person.PersonInChargeId = Guid.NewGuid();
                person.ProfileId = ProfileId;
                person.SalesEmployeeCode = EmployeeCode;
                person.RoleCode = ConstRoleCode.SALE_EMPLOYEE;
                person.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                person.CompanyCode = CompanyCode;
                person.CreateBy = SYSTEM;
                person.CreateTime = CreateTime;
                _context.Entry(person).State = EntityState.Added;
                //personList.Add(person);
            }

            //Sale Employee 2
            var EmployeeCode2 = item["SE_PERNR"].ToString();
            if (!string.IsNullOrEmpty(EmployeeCode2) && EmployeeCode2 != "00000000" && EmployeeCode2 != EmployeeCode && employeeList.Contains(EmployeeCode2))
            {
                PersonInChargeModel person = new PersonInChargeModel();
                person.PersonInChargeId = Guid.NewGuid();
                person.ProfileId = ProfileId;
                person.SalesEmployeeCode = EmployeeCode2;
                person.RoleCode = ConstRoleCode.SALE_EMPLOYEE2;
                person.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                person.CompanyCode = CompanyCode;
                person.CreateBy = SYSTEM;
                person.CreateTime = CreateTime;
                _context.Entry(person).State = EntityState.Added;
                //personList.Add(person);
            }
            //}
            #endregion
        }

        private void AddProfileGroup(DataRow item, string CompanyCode, Guid ProfileId)
        {
            #region ProfileGroupModel
            //Check tồn tại nhóm KH
            var existProfileGroup = _context.ProfileGroupModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
            if (existProfileGroup == null)
            {
                //Chưa có => Thêm mới
                if (!string.IsNullOrEmpty(item["KDGRP"].ToString()))
                {
                    ProfileGroupModel profileGroup = new ProfileGroupModel();
                    profileGroup.ProfileGroupId = Guid.NewGuid();
                    profileGroup.ProfileId = ProfileId;
                    profileGroup.ProfileGroupCode = item["KDGRP"].ToString();
                    profileGroup.CompanyCode = CompanyCode;
                    profileGroup.CreateBy = SYSTEM;
                    profileGroup.CreateTime = DateTime.Now;
                    _context.Entry(profileGroup).State = EntityState.Added;
                    //groupList.Add(profileGroup);
                }
            }
            else
            {
                //Cập nhật
                existProfileGroup.ProfileGroupCode = item["KDGRP"].ToString();
                existProfileGroup.LastEditBy = SYSTEM;
                existProfileGroup.LastEditTime = DateTime.Now;
                _context.Entry(existProfileGroup).State = EntityState.Modified;
            }
            #endregion
        }
        #endregion

        #region Sync Contact
        public void SyncContactFromSAP(DataTable dataTable)
        {
            var lst = new List<ProfileModel>();
            WriteLogFile(logFilePath, string.Format("Contact Count: {0}", dataTable.Rows.Count));

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    try
                    {
                        //Gán các field cần lưu
                        //1. Mã SAP
                        var ProfileForeignCode = item["KUNNR"].ToString();
                        //2. Danh xưng
                        var Title = item["TITLE_LH"].ToString();
                        if (string.IsNullOrEmpty(Title))
                        {
                            Title = null;
                        }
                        //3. Tên liên hệ
                        var ContactName = item["NAME_LH"].ToString();
                        //4. Điện thoại người liên hệ
                        var ContactNumber = item["PHONE_LH"].ToString().Replace("+84 ", "0").Trim().Replace(" ", "");
                        if (!string.IsNullOrEmpty(ContactNumber) && !string.IsNullOrEmpty(ProfileForeignCode) && !string.IsNullOrEmpty(ContactName) && ContactName != ".")
                        {
                            //5. Bộ phận người liên hệ
                            var DepartmentStr = item["BOPHAN_LH"].ToString();
                            string DepartmentCode = null;
                            var department = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Department && p.CatalogCode == DepartmentStr).FirstOrDefault();
                            if (department != null)
                            {
                                DepartmentCode = department.CatalogCode;
                            }
                            //6. Vị trí/chức năng nghiệp vụ
                            var PositionStr = item["CHUCNANG_LH"].ToString();
                            string PositionCode = null;
                            var position = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Position && p.CatalogCode == PositionStr).FirstOrDefault();
                            if (position != null)
                            {
                                PositionCode = position.CatalogCode;
                            }
                            //7. Email người liên hệ
                            var ContactEmail = item["EMAIL_LH"].ToString().Trim().Replace(" ", "").ToLower();
                            string Note = null;
                            //if (string.IsNullOrEmpty(ContactEmail))
                            //{
                            //    ContactEmail = null;
                            //}
                            //else if (IsValidEmail(ContactEmail) == false)
                            //{
                            //    Note = ContactEmail;
                            //    ContactEmail = null;
                            //}
                            //8. Địa chỉ người liên hệ
                            var ContactAddress = item["DIACHI_LH"].ToString();
                            //9. Ngày get data 
                            var UpdateDateStr = item["UPDDATE_LH"].ToString();
                            DateTime updateDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(UpdateDateStr))
                            {
                                updateDate = DateTime.ParseExact(UpdateDateStr, "yyyyMMdd", CultureInfo.InvariantCulture);
                            }

                            #region ProfileContactAttributeTempModel
                            //ProfileContactAttributeTempModel temp = new ProfileContactAttributeTempModel();
                            //temp.TempId = Guid.NewGuid();
                            //temp.ProfileForeignCode = ProfileForeignCode;
                            //temp.Title = Title;
                            //temp.ContactName = ContactName;
                            //temp.ContactNumber = ContactNumber;
                            //temp.Department = DepartmentStr;
                            //temp.Position = PositionStr;
                            //temp.ContactEmail = ContactEmail;
                            //temp.ContactAddress = ContactAddress;
                            //temp.UpdateDate = UpdateDateStr;

                            //_context.Entry(temp).State = EntityState.Added;
                            #endregion ProfileContactAttributeTempModel

                            //Contact Phone
                            //Account: Mã SAP
                            #region ProfileModel
                            ////Check khách hàng đã tồn tại chưa
                            //var existsProfile = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                            ////Nếu có tồn tại khách hàng thì mới thêm liên hệ
                            //if (existsProfile != null)
                            //{
                            //Check theo số điện thoại:
                            //nếu đã có rồi thì update, nếu chưa có thì thêm mới
                            var existsContact = (from p in _context.ProfileModel
                                                 join att in _context.ProfileContactAttributeModel on p.ProfileId equals att.ProfileId
                                                 join acc in _context.ProfileModel on att.CompanyId equals acc.ProfileId
                                                 where p.CustomerTypeCode == ConstCustomerType.Contact && p.Phone == ContactNumber
                                                       && acc.CustomerTypeCode == ConstCustomerType.Account && acc.ProfileForeignCode == ProfileForeignCode
                                                 select p).FirstOrDefault();


                            if (existsContact != null)
                            {
                                //Update 
                                existsContact.Title = Title;
                                existsContact.ProfileName = ContactName;
                                //if (!string.IsNullOrEmpty(ContactEmail))
                                //{
                                //    existsContact.Email = ContactEmail;
                                //}
                                //else
                                //{
                                //    if (!string.IsNullOrEmpty(existsContact.Email) && IsValidEmail(existsContact.Email) == false)
                                //    {
                                //        existsContact.Note = string.Format("(Email: {0})", existsContact.Email);
                                //        existsContact.Email = null;
                                //    }
                                //}
                                if (!string.IsNullOrEmpty(ContactEmail))
                                {
                                    try
                                    {
                                        //Delete all
                                        var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == existsContact.ProfileId).ToList();
                                        if (emailExistLst != null && emailExistLst.Count > 0)
                                        {
                                            _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                        }
                                        //Add again
                                        if (ContactEmail.Contains(";"))
                                        {
                                            var emailArray = ContactEmail.Split(';').ToList();
                                            foreach (var emailItem in emailArray)
                                            {
                                                if (IsValidEmail(emailItem) == true)
                                                {
                                                    ProfileEmailModel emailModel = new ProfileEmailModel();
                                                    emailModel.EmailId = Guid.NewGuid();
                                                    emailModel.ProfileId = existsContact.ProfileId;
                                                    emailModel.Email = emailItem.Trim();
                                                    _context.Entry(emailModel).State = EntityState.Added;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (IsValidEmail(ContactEmail) == true)
                                            {
                                                ProfileEmailModel emailModel = new ProfileEmailModel();
                                                emailModel.EmailId = Guid.NewGuid();
                                                emailModel.ProfileId = existsContact.ProfileId;
                                                emailModel.Email = ContactEmail.Trim();
                                                _context.Entry(emailModel).State = EntityState.Added;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // ghi log
                                        string mess = ex.Message;
                                        if (ex.InnerException != null)
                                        {
                                            mess = ex.InnerException.Message;
                                            if (ex.InnerException.InnerException != null)
                                            {
                                                mess = ex.InnerException.InnerException.Message;
                                            }
                                        }
                                        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                        continue;
                                    }
                                }
                                else
                                {
                                    existsContact.Email = null;
                                }
                                existsContact.Address = ContactAddress;
                                existsContact.LastEditBy = SYSTEM;
                                existsContact.LastEditTime = updateDate;

                                _context.Entry(existsContact).State = EntityState.Modified;

                                var existsContactAttr = _context.ProfileContactAttributeModel.Where(p => p.ProfileId == existsContact.ProfileId).FirstOrDefault();
                                if (existsContactAttr != null)
                                {
                                    existsContactAttr.DepartmentCode = DepartmentCode;
                                    existsContactAttr.Position = PositionCode;

                                    _context.Entry(existsContactAttr).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                var existsProfile = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                if (existsProfile != null)
                                {
                                    AddNewProfileContact(Title, ContactName, ContactNumber, DepartmentCode, PositionCode, ContactEmail, ContactAddress, updateDate, existsProfile.ProfileId, Note);
                                }
                            }
                            //}
                            #endregion ProfileModel

                            _context.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors (ProfileSAPCode: {2}, Email: {3}):",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State, item["KUNNR"].ToString(), item["EMAIL_LH"].ToString()));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                if (ve.PropertyName == "Email")
                                {
                                    //var ProfileForeignCode = item["KUNNR"].ToString();
                                    //var Phone = item["PHONE_LH"].ToString().Replace("+84 ", "0").Trim().Replace(" ", "");
                                    //var contact = (from p in _context.ProfileModel
                                    //               join att in _context.ProfileContactAttributeModel on p.ProfileId equals att.ProfileId
                                    //               join acc in _context.ProfileModel on att.CompanyId equals acc.ProfileId
                                    //               where p.CustomerTypeCode == ConstCustomerType.Contact && p.Phone == Phone
                                    //                     && acc.CustomerTypeCode == ConstCustomerType.Account && acc.ProfileForeignCode == ProfileForeignCode
                                    //               select p).FirstOrDefault();
                                    //if (contact != null)
                                    //{
                                    //    contact.Note = string.Format("(Email: {0})", contact.Email);
                                    //    contact.Email = null;
                                    //    _context.Entry(contact).State = EntityState.Modified;
                                    //    _context.SaveChanges();

                                    //    WriteLogFile(logFilePath, "Update Email => Note");
                                    //}
                                }
                                else
                                {
                                    WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                mess = ex.InnerException.Message;
                            }
                        }
                        //ghi log
                        //MessageBox.Show(mess);
                        WriteLogFile(logFilePath, "Sync Contact error (ProfileSAPCode: " + item["KUNNR"].ToString() + "): " + mess);
                    }
                }
            }
        }

        bool IsValidEmail(string email)
        {
            //try
            //{
            //    var addr = new MailAddress(email);
            //    return addr.Address == email;
            //}
            //catch
            //{
            //    return false;
            //}
            string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                                          @"@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            return Regex.IsMatch(email, RegexPattern, RegexOptions.IgnoreCase);
        }

        private void AddNewProfileContact(string Title, string ContactName, string ContactNumber, string DepartmentCode, string PositionCode, string ContactEmail, string ContactAddress, DateTime updateDate, Guid CompanyId, string Note)
        {
            ProfileModel profileContact = new ProfileModel();
            profileContact.ProfileId = Guid.NewGuid();
            //profileContact.isForeignCustomer = existsProfile.isForeignCustomer;
            profileContact.CustomerTypeCode = ConstCustomerType.Contact;
            profileContact.Title = Title;
            profileContact.ProfileName = ContactName;
            profileContact.Phone = ContactNumber;
            //profileContact.Email = ContactEmail;
            if (!string.IsNullOrEmpty(ContactEmail))
            {
                try
                {
                    //Delete all
                    var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == profileContact.ProfileId).ToList();
                    if (emailExistLst != null && emailExistLst.Count > 0)
                    {
                        _context.ProfileEmailModel.RemoveRange(emailExistLst);
                    }
                    //Add again
                    if (ContactEmail.Contains(";"))
                    {
                        var emailArray = ContactEmail.Split(';').ToList();
                        foreach (var emailItem in emailArray)
                        {
                            if (IsValidEmail(emailItem) == true)
                            {
                                ProfileEmailModel emailModel = new ProfileEmailModel();
                                emailModel.EmailId = Guid.NewGuid();
                                emailModel.ProfileId = profileContact.ProfileId;
                                emailModel.Email = emailItem.Trim();
                                _context.Entry(emailModel).State = EntityState.Added;
                            }
                        }
                    }
                    else
                    {
                        if (IsValidEmail(ContactEmail) == true)
                        {
                            ProfileEmailModel emailModel = new ProfileEmailModel();
                            emailModel.EmailId = Guid.NewGuid();
                            emailModel.ProfileId = profileContact.ProfileId;
                            emailModel.Email = ContactEmail.Trim();
                            _context.Entry(emailModel).State = EntityState.Added;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ghi log
                    string mess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        mess = ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            mess = ex.InnerException.InnerException.Message;
                        }
                    }
                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                }
            }
            else
            {
                profileContact.Email = null;
            }
            profileContact.Address = ContactAddress;
            profileContact.Actived = true;
            //profileContact.CreateAtCompany = existsProfile.CreateAtCompany;
            //profileContact.CreateAtSaleOrg = existsProfile.CreateAtSaleOrg;
            profileContact.CreateBy = SYSTEM;
            profileContact.CreateTime = updateDate;
            profileContact.Note = Note;

            _context.Entry(profileContact).State = EntityState.Added;

            ProfileContactAttributeModel contactAttr = new ProfileContactAttributeModel();
            contactAttr.ProfileId = profileContact.ProfileId;
            contactAttr.CompanyId = CompanyId;
            contactAttr.Position = PositionCode;
            contactAttr.DepartmentCode = DepartmentCode;

            _context.Entry(contactAttr).State = EntityState.Added;
        }
        #endregion Sync Contact

        #region Sync Credit Limit
        public void SyncCreditLimit(DataTable dataTable)
        {
            var lst = new List<CreditLimitModel>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    try
                    {
                        var SaleOrgCode = item["VKORG"].ToString();
                        var ProfileForeignCode = item["KUNNR"].ToString();
                        UtilitiesRepository _utilities = new UtilitiesRepository();
                        var FromDate = _utilities.ConvertDateTime(item["FRDATE"].ToString());
                        var ToDate = _utilities.ConvertDateTime(item["TODATE"].ToString());
                        var exist = _context.CreditLimitModel
                                            .Where(p => p.SaleOrgCode == SaleOrgCode &&
                                                        p.ProfileForeignCode == ProfileForeignCode &&
                                                        p.FromDate == FromDate)
                                            .FirstOrDefault();
                        if (exist == null)
                        {
                            CreditLimitModel model = new CreditLimitModel();
                            model.CreditLimitId = Guid.NewGuid();
                            model.ProfileForeignCode = ProfileForeignCode;
                            model.CompanyCode = item["WERKS"].ToString();
                            model.SaleOrgCode = SaleOrgCode;
                            model.FromDate = FromDate;
                            model.ToDate = ToDate;
                            model.Amount = Convert.ToInt32(item["AMOUNT"].ToString());
                            _context.Entry(model).State = EntityState.Added;
                        }
                        else
                        {
                            exist.ToDate = ToDate;
                            exist.Amount = Convert.ToInt32(item["AMOUNT"].ToString());
                            _context.Entry(exist).State = EntityState.Modified;
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                mess = ex.InnerException.Message;
                            }
                        }
                        //ghi log
                        //MessageBox.Show(mess);
                        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                    }
                }
                //_context.SaveChanges();
            }
        }
        #endregion

        #region Sync Material
        public void SyncMaterial(int type)
        {
            List<string> materialTypeLst = new List<string>();
            var dt = new DataTable();

            if (type == 1 || type == 2 || type == 3)
            {
                var categoryList = _context.CategoryModel.Where(p => p.Actived == true).AsNoTracking().ToList();

                var companyLst = _context.CompanyModel.Where(p => p.Actived == true).ToList();
                if (companyLst != null && companyLst.Count > 0)
                {
                    //Product
                    if (type == 1)
                    {
                        #region Get Datatable
                        foreach (var item in companyLst)
                        {
                            //Đồng bộ lần đầu: Lấy theo Material Type (Zxx)
                            if (FirstSync == 1)
                            {
                                if (item.CompanyCode == ConstCompanyCode.AnCuong)
                                {
                                    materialTypeLst = new List<string>();
                                    materialTypeLst.Add("Z23");
                                    materialTypeLst.Add("Z51");
                                    materialTypeLst.Add("Z54");
                                    materialTypeLst.Add("Z80");

                                    foreach (var matType in materialTypeLst)
                                    {
                                        var data = GetDataMaterial(type, item.CompanyCode, matType);
                                        dt.Merge(data);
                                    }
                                }
                                else if (item.CompanyCode == ConstCompanyCode.Malloca)
                                {
                                    materialTypeLst = new List<string>();
                                    materialTypeLst.Add("Z40");
                                    materialTypeLst.Add("Z52");
                                    materialTypeLst.Add("Z60");
                                    materialTypeLst.Add("Z61");
                                    materialTypeLst.Add("Z62");
                                    materialTypeLst.Add("Z63");
                                    materialTypeLst.Add("Z81");
                                    materialTypeLst.Add("Z90");
                                    materialTypeLst.Add("Z91");
                                    materialTypeLst.Add("Z92");
                                    materialTypeLst.Add("Z99");

                                    foreach (var matType in materialTypeLst)
                                    {
                                        var data = GetDataMaterial(type, item.CompanyCode, matType);
                                        dt.Merge(data);
                                    }
                                }
                                else if (item.CompanyCode == ConstCompanyCode.Aconcept)
                                {
                                    materialTypeLst = new List<string>();
                                    materialTypeLst.Add("Z63");
                                    materialTypeLst.Add("Z64");
                                    materialTypeLst.Add("Z65");
                                    materialTypeLst.Add("Z66");
                                    materialTypeLst.Add("Z83");
                                    materialTypeLst.Add("Z84");

                                    foreach (var matType in materialTypeLst)
                                    {
                                        var data = GetDataMaterial(type, item.CompanyCode, matType);
                                        dt.Merge(data);
                                    }
                                }
                                else if (item.CompanyCode == ConstCompanyCode.AnCuong_NhaXuong)
                                {

                                }
                            }
                            //Đồng bộ theo ngày: Lấy theo ngày: %
                            else
                            {
                                var data = GetDataMaterial(type, item.CompanyCode, "%");
                                dt.Merge(data);
                            }
                        }
                        #endregion

                        #region Add data into db
                        if (FirstSync == 1)
                        {
                            var existList = _context.ProductModel.Select(p => p.ProductCode).AsNoTracking().ToList();
                            if (existList != null && existList.Count > 0)
                            {
                                var rows = dt.AsEnumerable().Where(p => !existList.Contains(p.Field<string>("MATNR")));
                                if (rows.Any())
                                {
                                    dt = rows.CopyToDataTable();
                                }
                                else
                                {
                                    dt = new DataTable();
                                }
                            }
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            try
                            {
                                List<ProductModel> productList = new List<ProductModel>();
                                List<ProductAttributeModel> productAttrList = new List<ProductAttributeModel>();

                                foreach (DataRow item in dt.Rows)
                                {
                                    var ProductId = Guid.NewGuid();
                                    var ProductCode = item["MATNR"].ToString();
                                    var CompanyId = Guid.Parse(item["CompanyId"].ToString());
                                    var WarrantyCode = string.Format("BH{0}T", item["WARRANTY"].ToString());
                                    var WarrantyId = _context.WarrantyModel.Where(p => p.WarrantyCode == WarrantyCode).Select(p => p.WarrantyId).FirstOrDefault();

                                    //ParentCategory
                                    var ParentCategoryCode = item["MTART"].ToString();
                                    Guid? ParentCategoryId = categoryList.Where(p => p.CategoryCode == ParentCategoryCode)
                                                                         .Select(p => p.CategoryId).FirstOrDefault();
                                    if (ParentCategoryId == Guid.Empty)
                                    {
                                        ParentCategoryId = null;
                                    }
                                    //Category
                                    var CategoryCode = item["MATKL"].ToString();
                                    Guid? CategoryId = categoryList.Where(p => p.CategoryCode == CategoryCode)
                                                                   .Select(p => p.CategoryId).FirstOrDefault();
                                    if (CategoryId == Guid.Empty)
                                    {
                                        CategoryId = null;
                                    }

                                    #region Product
                                    var exist = _context.ProductModel
                                                        .Where(p => p.ProductCode == ProductCode && p.CompanyId == CompanyId)
                                                        .FirstOrDefault();
                                    if (exist == null)
                                    {
                                        ProductModel model = new ProductModel();
                                        model.ProductId = ProductId;
                                        model.ProductCode = ProductCode;
                                        model.ERPProductCode = ProductCode;
                                        model.ProductName = item["MAKTX"].ToString();
                                        model.ParentCategoryId = ParentCategoryId;
                                        model.CategoryId = CategoryId;
                                        model.ProductName = item["MAKTX"].ToString();
                                        model.Actived = true;
                                        model.CompanyId = CompanyId;
                                        model.WarrantyId = WarrantyId;
                                        //_context.Entry(model).State = EntityState.Added;
                                        productList.Add(model);
                                    }
                                    else
                                    {
                                        ProductId = exist.ProductId;
                                        exist.ProductName = item["MAKTX"].ToString();
                                        exist.ParentCategoryId = ParentCategoryId;
                                        exist.CategoryId = CategoryId;
                                        exist.WarrantyId = WarrantyId;
                                    }
                                    #endregion

                                    #region Product Attribute
                                    //Delete exist attribute
                                    var existAttr = _context.ProductAttributeModel.Where(p => p.ProductId == ProductId).FirstOrDefault();
                                    if (existAttr != null)
                                    {
                                        _context.Entry(existAttr).State = EntityState.Deleted;
                                    }
                                    //Add new again
                                    ProductAttributeModel attr = new ProductAttributeModel();
                                    attr.ProductId = ProductId;
                                    attr.Description = item["MAKTXDESC"].ToString();
                                    attr.Unit = item["MEINS"].ToString();
                                    attr.Color = item["EXTWG"].ToString();
                                    attr.Thickness = item["LABOR"].ToString();
                                    attr.Allocation = item["KOSCH"].ToString();
                                    attr.Grade = item["MEDIUM"].ToString();
                                    attr.Surface = item["ZEIFO"].ToString();
                                    attr.NumberOfSurface = item["BLANZ"].ToString();
                                    attr.GrossWeight = Convert.ToDecimal(item["BRGEW"].ToString());
                                    attr.NetWeight = Convert.ToDecimal(item["NTGEW"].ToString());
                                    attr.WeightUnit = item["GEWEI"].ToString();
                                    //_context.Entry(attr).State = EntityState.Added;
                                    productAttrList.Add(attr);
                                    #endregion
                                }
                                if (productList != null && productList.Count > 0)
                                {
                                    _context.ProductModel.AddRange(productList);
                                }
                                _context.ProductAttributeModel.AddRange(productAttrList);
                                _context.SaveChanges();
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var mess = ex.Message;
                                if (ex.InnerException != null)
                                {
                                    if (ex.InnerException.InnerException != null)
                                    {
                                        mess = ex.InnerException.InnerException.Message;
                                    }
                                    else
                                    {
                                        mess = ex.InnerException.Message;
                                    }
                                }
                                //ghi log
                                //MessageBox.Show(mess);
                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                            }
                        }
                        #endregion
                    }
                    //Category
                    else if (type == 2 || type == 3)
                    {
                        #region Category
                        dt = GetDataMaterial(type, "", "%");
                        if (FirstSync == 1)
                        {
                            var existList = _context.CategoryModel.Select(p => p.CategoryCode).AsNoTracking().ToList();
                            if (existList != null && existList.Count > 0)
                            {
                                var CategoryCode = type == 2 ? "MTART" : "MATKL";
                                var rows = dt.AsEnumerable().Where(p => !existList.Contains(p.Field<string>(CategoryCode)));
                                if (rows.Any())
                                {
                                    dt = rows.CopyToDataTable();
                                }
                                else
                                {
                                    dt = new DataTable();
                                }
                            }
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            try
                            {
                                List<CategoryModel> newCategoryList = new List<CategoryModel>();

                                var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.PHANLOAIVATTU).FirstOrDefault();
                                var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

                                foreach (DataRow item in dt.Rows)
                                {
                                    var CategoryCode = type == 2 ? item["MTART"].ToString() : item["MATKL"].ToString();
                                    var CategoryName = type == 2 ? item["MTBEZ"].ToString() : item["WGBEZ60"].ToString();

                                    if (!string.IsNullOrEmpty(CategoryCode))
                                    {
                                        var exist = _context.CategoryModel
                                                            .Where(p => p.CategoryCode == CategoryCode)
                                                            .FirstOrDefault();
                                        if (exist == null)
                                        {
                                            CategoryModel model = new CategoryModel();
                                            model.CategoryId = Guid.NewGuid();
                                            model.CategoryCode = CategoryCode;
                                            model.CategoryName = CategoryName;
                                            model.Actived = true;
                                            #region CHUAPHANLOAI (not use)
                                            //if (type == 3)
                                            //{
                                            //    var category = CategoryCode.Length >= 2 ? CategoryCode.Substring(0, 2) : CategoryCode;
                                            //    int n;
                                            //    var isNumber = int.TryParse(category, out n);
                                            //    if (isNumber)
                                            //    {

                                            //        var parent = _context.CategoryModel.Where(p => p.CategoryCode == "Z" + category)
                                            //                             .FirstOrDefault();
                                            //        if (parent != null)
                                            //        {
                                            //            model.ParentCategoryId = parent.CategoryId;
                                            //        }
                                            //    }
                                            //    //Nếu không có phân loại xếp vào nhóm CHUAPHANLOAI
                                            //    if (model.ParentCategoryId == null)
                                            //    {
                                            //        var noTypeCatergory = _context.CategoryModel
                                            //                                      .Where(p => p.CategoryCode == "CHUAPHANLOAI")
                                            //                                      .Select(p => p.CategoryId)
                                            //                                      .FirstOrDefault();
                                            //        model.ParentCategoryId = noTypeCatergory;
                                            //    }
                                            //}
                                            #endregion
                                            if (type == 2)
                                            {
                                                model.ParentCategoryId = PhanLoaiVatTu.CategoryId;
                                            }
                                            else
                                            {
                                                model.ParentCategoryId = NhomVatTu.CategoryId;
                                            }
                                            //_context.Entry(model).State = EntityState.Added;
                                            newCategoryList.Add(model);
                                        }
                                        else
                                        {
                                            exist.CategoryName = CategoryName;
                                            #region CHUAPHANLOAI (not use)
                                            //if (type == 3)
                                            //{
                                            //    var category = CategoryCode.Length >= 2 ? CategoryCode.Substring(0, 2) : CategoryCode;
                                            //    int n;
                                            //    var isNumber = int.TryParse(category, out n);
                                            //    if (isNumber)
                                            //    {

                                            //        var parent = _context.CategoryModel.Where(p => p.CategoryCode == "Z" + category)
                                            //                             .FirstOrDefault();
                                            //        if (parent != null)
                                            //        {
                                            //            exist.ParentCategoryId = parent.CategoryId;
                                            //        }
                                            //    }
                                            //    //Nếu không có phân loại xếp vào nhóm CHUAPHANLOAI
                                            //    if (exist.ParentCategoryId == null)
                                            //    {
                                            //        var noTypeCatergory = _context.CategoryModel
                                            //                                      .Where(p => p.CategoryCode == "CHUAPHANLOAI")
                                            //                                      .Select(p => p.CategoryId)
                                            //                                      .FirstOrDefault();
                                            //        exist.ParentCategoryId = noTypeCatergory;
                                            //    }
                                            //}
                                            #endregion
                                            if (type == 2)
                                            {
                                                exist.ParentCategoryId = PhanLoaiVatTu.CategoryId;
                                            }
                                            else
                                            {
                                                exist.ParentCategoryId = NhomVatTu.CategoryId;
                                            }
                                        }
                                    }
                                }
                                if (newCategoryList != null && newCategoryList.Count > 0)
                                {
                                    _context.CategoryModel.AddRange(newCategoryList);
                                }
                                _context.SaveChanges();
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var mess = ex.Message;
                                if (ex.InnerException != null)
                                {
                                    if (ex.InnerException.InnerException != null)
                                    {
                                        mess = ex.InnerException.InnerException.Message;
                                    }
                                    else
                                    {
                                        mess = ex.InnerException.Message;
                                    }
                                }
                                //ghi log
                                //MessageBox.Show(mess);
                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                            }
                        }
                        #endregion
                    }
                }
            }
            //ProductLevel (Phân cấp sản phẩm)
            else if (type == 4)
            {
                #region ProductLevel
                dt = GetDataMaterial(type, "", "%");
                if (FirstSync == 1)
                {
                    var existList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.ProductLevel)
                                            .Select(p => p.CatalogCode).AsNoTracking().ToList();
                    if (existList != null && existList.Count > 0)
                    {
                        var rows = dt.AsEnumerable().Where(p => !existList.Contains(p.Field<string>("PRDHA")));
                        if (rows.Any())
                        {
                            dt = rows.CopyToDataTable();
                        }
                        else
                        {
                            dt = new DataTable();
                        }
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            var ProductLevelCode = item["PRDHA"].ToString();
                            if (!string.IsNullOrEmpty(ProductLevelCode))
                            {
                                Nullable<int> OrderIndex = null;
                                if (!string.IsNullOrEmpty(item["WARRANTY"].ToString()))
                                {
                                    OrderIndex = Convert.ToInt32(item["WARRANTY"].ToString());
                                }
                                var existCode = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.ProductLevel && p.CatalogCode == ProductLevelCode).FirstOrDefault();
                                if (existCode != null)
                                {
                                    //Tên phân cấp sản phẩm
                                    existCode.CatalogText_vi = item["PRDHANAME"].ToString();
                                    //Đơn vị tính thời gian BH
                                    existCode.CatalogText_en = item["WARRDVT"].ToString();
                                    //Số thời gian BH
                                    existCode.OrderIndex = OrderIndex > 0 ? OrderIndex : null;
                                }
                                else
                                {
                                    CatalogModel newProductLevel = new CatalogModel()
                                    {
                                        CatalogId = Guid.NewGuid(),
                                        CatalogTypeCode = ConstCatalogType.ProductLevel,
                                        CatalogCode = ProductLevelCode,
                                        CatalogText_vi = item["PRDHANAME"].ToString(),
                                        CatalogText_en = item["WARRDVT"].ToString(),
                                        OrderIndex = OrderIndex > 0 ? OrderIndex : null,
                                        Actived = true
                                    };
                                    _context.CatalogModel.Add(newProductLevel);
                                }
                                _context.SaveChanges();
                            }
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                mess = ex.InnerException.Message;
                            }
                        }
                        //ghi log
                        //MessageBox.Show(mess);
                        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                    }
                }
                #endregion
            }
            //ProductColor (Mã màu)
            else if (type == 5)
            {
                #region ProductColor
                dt = GetDataMaterial(type, "", "%");
                if (FirstSync == 1)
                {
                    var existList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.ProductColor)
                                            .Select(p => p.CatalogCode).AsNoTracking().ToList();
                    if (existList != null && existList.Count > 0)
                    {
                        var rows = dt.AsEnumerable().Where(p => !existList.Contains(p.Field<string>("EXTWG")));
                        if (rows.Any())
                        {
                            dt = rows.CopyToDataTable();
                        }
                        else
                        {
                            dt = new DataTable();
                        }
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            var ProductColorCode = item["EXTWG"].ToString();
                            if (!string.IsNullOrEmpty(ProductColorCode))
                            {
                                var existCode = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.ProductColor && p.CatalogCode == ProductColorCode).FirstOrDefault();
                                if (existCode == null)
                                {
                                    CatalogModel newProductColor = new CatalogModel()
                                    {
                                        CatalogId = Guid.NewGuid(),
                                        CatalogTypeCode = ConstCatalogType.ProductColor,
                                        CatalogCode = ProductColorCode,
                                        Actived = true
                                    };
                                    _context.CatalogModel.Add(newProductColor);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                mess = ex.InnerException.Message;
                            }
                        }
                        //ghi log
                        //MessageBox.Show(mess);
                        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                    }
                }
                #endregion
            }
        }
        #endregion

        #region WriteLogFile
        public static void WriteLogFile(string filePath, string message)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Create(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Close();
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
                sw.WriteLine(lastRecordText);
                sw.Close();
            }
        }
        #endregion

        private void frmSyncData_Load(object sender, EventArgs e)
        {
            //if (isDebug == false)
            //{
            //    //btnSyncData.PerformClick();
            //    //btnSyncProfile.PerformClick();
            //    //btnSyncMaterial.PerformClick();
            //    //btnSyncMaterialDetails.PerformClick();
            //}
            //else
            //{
            //    //btnSyncContact.PerformClick();
            //}
            btnSyncProfile.PerformClick();
        }

        private void frmSyncData_Shown(object sender, EventArgs e)
        {
            btnSyncProfile_Click(sender, e);
        }

        #region Đồng bộ khách hàng theo phân nhóm
        private void btnSyncProfileWithCAG_Click(object sender, EventArgs e)
        {
            if (btnSyncProfileWithCAG.Text == "SYNC PROFILE WITH CAG")
            {
                btnSyncProfileWithCAG.Text = "SYNC PROFILE WITH CAG PROCESSING";
                btnSyncProfileWithCAG.BackColor = Color.DarkRed;

                WriteLogFile(logFilePath, "SYNC PROFILE WITH CAG: Start.");
                RunFunction(15);
            }
            else if (btnSyncProfileWithCAG.Text == "SYNC PROFILE WITH CAG PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btnSyncProfileWithCAG.Text = "SYNC PROFILE WITH CAG";
                    btnSyncProfileWithCAG.BackColor = Color.DeepSkyBlue;
                }
            }
        }

        #region Sync Profile With CAG
        public void SyncProfileWithCAGFromSAP(DataTable dataTable)
        {
            var lst = new List<ProfileModel>();
            WriteLogFile(logFilePath, string.Format("Profile Count: {0}", dataTable.Rows.Count));
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                try
                {
                    //Tỉnh thành
                    var provinceLst = _context.ProvinceModel.Where(p => p.Actived == true).AsNoTracking().ToList();
                    //Quận huyện
                    var districtLst = _context.DistrictModel.Where(p => p.Actived == true && p.ProvinceId != null && p.DistrictName != null).AsNoTracking().ToList();
                    //Nhân viên (SalesEmployee)
                    var employeeList = _context.SalesEmployeeModel.Select(p => p.SalesEmployeeCode).AsNoTracking().ToList();

                    //_context.Configuration.AutoDetectChangesEnabled = false;
                    List<ProfileModel> profileList = new List<ProfileModel>();
                    List<string> profileCodeList = new List<string>();
                    List<ProfilePhoneModel> phoneList = new List<ProfilePhoneModel>();
                    List<ProfileBAttributeModel> bAttrList = new List<ProfileBAttributeModel>();
                    List<ProfileContactAttributeModel> contactList = new List<ProfileContactAttributeModel>();
                    List<PersonInChargeModel> personList = new List<PersonInChargeModel>();
                    List<RoleInChargeModel> roleList = new List<RoleInChargeModel>();
                    List<AddressBookModel> addressList = new List<AddressBookModel>();
                    List<ProfileTypeModel> typeList = new List<ProfileTypeModel>();
                    List<ProfileGroupModel> groupList = new List<ProfileGroupModel>();
                    //List<ExistProfileModel> existList = new List<ExistProfileModel>();

                    foreach (DataRow item in dataTable.Rows)
                    {
                        try
                        {
                            //Mã SAP
                            var ProfileForeignCode = item["KUNNR"].ToString();
                            //Mã CRM
                            var ProfileCode_Str = item["LOCCO"].ToString();
                            int ProfileCode = 0;
                            if (!string.IsNullOrEmpty(ProfileCode_Str))
                            {
                                ProfileCode = Convert.ToInt32(item["LOCCO"].ToString());
                            }
                            //Mã công ty
                            var CompanyCode = item["BUKRS"].ToString();
                            //Mã SAP lớn hơn 8 ký tự là loại địa chỉ
                            bool isAddressBook = false;
                            //if (ProfileForeignCode.Length > 8)
                            //{
                            //    isAddressBook = true;
                            //}
                            //Tạo mới ProfileId
                            var ProfileId = Guid.NewGuid();
                            //DateTime
                            UtilitiesRepository _utilities = new UtilitiesRepository();
                            var CreateTime = _utilities.ConvertDateTime(item["ERDAT"].ToString());
                            var Phone = item["TEL_NUMBER"].ToString();
                            var TaxNo = item["STCEG"].ToString();
                            //Trong nước | Nước ngoài: 
                            //1. Nếu LAND1 == "VN" => trong nước
                            //2. Nếu LAND1 != "VN" => nước ngoài
                            bool isForeignCustomer = false;
                            string CountryCode = item["LAND1"].ToString();
                            if (!string.IsNullOrEmpty(CountryCode) && CountryCode != "VN")
                            {
                                isForeignCustomer = true;
                            }
                            //Tỉnh thành
                            var ProvinceCode = item["VKGRP"].ToString();
                            Guid? ProvinceId = null;
                            if (!string.IsNullOrEmpty(ProvinceCode))
                            {
                                ProvinceId = provinceLst.Where(p => p.ProvinceCode == ProvinceCode)
                                                        .Select(p => p.ProvinceId).FirstOrDefault();
                            }
                            else
                            {
                                //Nếu không có Tỉnh thành => Lưu "Khác"
                                ProvinceId = Guid.Parse("33b02d3c-c3dc-41ca-a909-09c951b9ad22");
                            }
                            //Quận huyện
                            var DistrictCode = item["BZIRK"].ToString();
                            Guid? DistrictId = null;
                            if (!string.IsNullOrEmpty(DistrictCode) && DistrictCode != "999999")
                            {
                                DistrictId = districtLst.Where(p => p.DistrictCode == DistrictCode)
                                                        .Select(p => p.DistrictId).FirstOrDefault();
                            }
                            //Phân loại khách hàng: 1.KH Tiêu dùng. 2.KH Doanh nghiệp
                            var PhanLoaiKH = item["NHOMKH"].ToString();
                            var ProfileTypeCode = string.Empty;
                            if (!string.IsNullOrEmpty(PhanLoaiKH))
                            {
                                if (PhanLoaiKH == "1")
                                {
                                    ProfileTypeCode = ConstCustomerType.Customer;
                                }
                                else if (PhanLoaiKH == "2")
                                {
                                    ProfileTypeCode = ConstCustomerType.Bussiness;
                                }
                            }

                            #region Profile
                            //Check profile exists có mã CRM
                            var existProfileWithCRMCode = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileCode == ProfileCode).FirstOrDefault();
                            //Nếu đã có profile trong DB và chưa có Mã SAP => cập nhật
                            if (existProfileWithCRMCode != null
                                //Chưa có mã SAP thì cập nhật
                                && string.IsNullOrEmpty(existProfileWithCRMCode.ProfileForeignCode))
                            {
                                if (isAddressBook == false)
                                {
                                    //Nếu mã SAP đã tồn tại thì không làm gì hết
                                    var existsSAPProfile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).Select(p => new
                                    {
                                        p.ProfileCode,
                                        p.ProfileForeignCode
                                    }).FirstOrDefault();
                                    //CRM chưa có mã SAP thì mới cập nhật
                                    if (existsSAPProfile == null)
                                    {
                                        //1. GUID
                                        ProfileId = existProfileWithCRMCode.ProfileId;
                                        //2. ProfileCode
                                        if (ProfileCode != 0)
                                        {
                                            existProfileWithCRMCode.ProfileCode = ProfileCode;
                                        }
                                        //3. ProfileForeignCode
                                        existProfileWithCRMCode.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        existProfileWithCRMCode.isForeignCustomer = isForeignCustomer;
                                        existProfileWithCRMCode.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        existProfileWithCRMCode.Title = Title;
                                        ////6. Loại
                                        existProfileWithCRMCode.CustomerTypeCode = ConstCustomerType.Account;

                                        //Phân loại KH
                                        var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                        if (existProfileType == null)
                                        {
                                            ProfileTypeModel profileType = new ProfileTypeModel();
                                            profileType.ProfileTypeId = Guid.NewGuid();
                                            profileType.ProfileId = ProfileId;
                                            //if (existProfileWithCRMCode.Title == "Company")
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                            //}
                                            //else
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                            //}
                                            profileType.CustomerTypeCode = ProfileTypeCode;
                                            profileType.CompanyCode = item["BUKRS"].ToString();
                                            profileType.CreateBy = SYSTEM;
                                            profileType.CreateTime = CreateTime;
                                            _context.Entry(profileType).State = EntityState.Added;
                                            //typeList.Add(profileType);
                                        }
                                        else
                                        {
                                            existProfileType.CustomerTypeCode = ProfileTypeCode;
                                            _context.Entry(existProfileType).State = EntityState.Modified;
                                        }
                                        //7. Họ va Tên|Tên công ty
                                        existProfileWithCRMCode.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        existProfileWithCRMCode.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        existProfileWithCRMCode.AbbreviatedName = existProfileWithCRMCode.ProfileName?.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        try
                                        {
                                            //Delete all
                                            var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                            if (phoneExistLst != null && phoneExistLst.Count > 0)
                                            {
                                                _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                            }
                                            //Add again
                                            if (!string.IsNullOrEmpty(Phone))
                                            {
                                                if (Phone.Contains("-"))
                                                {
                                                    var arr = Phone.Split('-').ToList();
                                                    var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                                    var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                    foreach (var phoneItem in phoneArray)
                                                    {
                                                        ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                        phoneModel.PhoneId = Guid.NewGuid();
                                                        phoneModel.ProfileId = ProfileId;
                                                        phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                        if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                        {
                                                            _context.Entry(phoneModel).State = EntityState.Added;
                                                            //phoneList.Add(phoneModel);
                                                        }
                                                    }
                                                    existProfileWithCRMCode.Phone = PhoneNumber.Trim();
                                                }
                                                else
                                                {
                                                    existProfileWithCRMCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                                }
                                            }
                                            else
                                            {
                                                existProfileWithCRMCode.Phone = null;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // ghi log
                                            string mess = ex.Message;
                                            if (ex.InnerException != null)
                                            {
                                                mess = ex.InnerException.Message;
                                                if (ex.InnerException.InnerException != null)
                                                {
                                                    mess = ex.InnerException.InnerException.Message;
                                                }
                                            }
                                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                        }
                                        //15. Email
                                        //16. Khu vực
                                        existProfileWithCRMCode.SaleOfficeCode = item["VKBUR"].ToString();
                                        //17. Địa chỉ
                                        existProfileWithCRMCode.Address = item["ADDRESS"].ToString();
                                        //18. Tỉnh/Thành phố
                                        existProfileWithCRMCode.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        existProfileWithCRMCode.DistrictId = DistrictId;
                                        //20. Phường/Xã
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái (not update this field)
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty 
                                        existProfileWithCRMCode.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        existProfileWithCRMCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy (not update this field)
                                        //29. Thời gian tạo
                                        //exist.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        existProfileWithCRMCode.LastEditBy = SYSTEM;
                                        //31. Thời gian sửa
                                        existProfileWithCRMCode.LastEditTime = DateTime.Now;
                                        //32. Phân nhóm KH/ Customer Account Group
                                        existProfileWithCRMCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        existProfileWithCRMCode.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        existProfileWithCRMCode.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        existProfileWithCRMCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        existProfileWithCRMCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        existProfileWithCRMCode.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                        existProfileWithCRMCode.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            existProfileWithCRMCode.TaxNo = TaxNo;
                                        }
                                        //nguồn KH
                                        existProfileWithCRMCode.CustomerSourceCode = "SHOWROOM";
                                        if (!string.IsNullOrEmpty(existProfileWithCRMCode.Email) && IsValidEmail(existProfileWithCRMCode.Email) == false)
                                        {
                                            existProfileWithCRMCode.Note = string.Format("(Email: {0})", existProfileWithCRMCode.Email);
                                            existProfileWithCRMCode.Email = null;
                                        }
                                        _context.Entry(existProfileWithCRMCode).State = EntityState.Modified;

                                        //thêm nhóm KH
                                        AddProfileGroup(item, CompanyCode, ProfileId);

                                        //thêm NVKD
                                        AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);
                                    }
                                    else
                                    {
                                        WriteLogFile(logFilePath, "Sync Profile error: Update CRM: " + ProfileCode + " - Voi SAP: " + ProfileForeignCode + ". Nhung da Ton tai:" + existsSAPProfile.ProfileCode);
                                    }
                                    //profileCodeList.Add(ProfileForeignCode);
                                }
                                //cập nhật address book
                                else
                                {

                                }
                            }


                            //Nếu không phải là yêu cầu cập nhật => tìm theo mã SAP
                            else if (existProfileWithCRMCode == null)
                            {
                                //Tìm theo mã SAP
                                var existProfileWithSAPCode = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                if (existProfileWithSAPCode == null)
                                {
                                    if (isAddressBook == false)
                                    {
                                        ProfileModel model = new ProfileModel();
                                        //1. GUID
                                        model.ProfileId = ProfileId;
                                        //2. ProfileCode
                                        if (ProfileCode != 0)
                                        {
                                            model.ProfileCode = ProfileCode;
                                        }
                                        //3. ProfileForeignCode
                                        model.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        model.isForeignCustomer = isForeignCustomer;
                                        model.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        model.Title = Title;
                                        //6. Loại
                                        model.CustomerTypeCode = ConstCustomerType.Account;

                                        #region  //Phân loại KH
                                        ProfileTypeModel profileType = new ProfileTypeModel();
                                        profileType.ProfileTypeId = Guid.NewGuid();
                                        profileType.ProfileId = ProfileId;
                                        //if (model.Title == "Company")
                                        //{
                                        //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                        //}
                                        //else
                                        //{
                                        //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                        //}
                                        profileType.CustomerTypeCode = ProfileTypeCode;
                                        profileType.CompanyCode = item["BUKRS"].ToString();
                                        profileType.CreateBy = SYSTEM;
                                        profileType.CreateTime = CreateTime;
                                        _context.Entry(profileType).State = EntityState.Added;
                                        #endregion
                                        //typeList.Add(profileType);
                                        //7. Họ va Tên|Tên công ty
                                        model.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        model.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        model.AbbreviatedName = model.ProfileName.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Phone))
                                            {
                                                if (Phone.Contains("-"))
                                                {
                                                    var arr = Phone.Split('-').ToList();
                                                    var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);
                                                    var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                    foreach (var phoneItem in phoneArray)
                                                    {
                                                        ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                        phoneModel.PhoneId = Guid.NewGuid();
                                                        phoneModel.ProfileId = ProfileId;
                                                        phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                        if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                        {
                                                            _context.Entry(phoneModel).State = EntityState.Added;
                                                            //phoneList.Add(phoneModel);
                                                        }
                                                    }
                                                    model.Phone = PhoneNumber.Trim();
                                                }
                                                else
                                                {
                                                    model.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone).Trim();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // ghi log
                                            string mess = ex.Message;
                                            if (ex.InnerException != null)
                                            {
                                                mess = ex.InnerException.Message;
                                                if (ex.InnerException.InnerException != null)
                                                {
                                                    mess = ex.InnerException.InnerException.Message;
                                                }
                                            }
                                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                            continue;
                                        }
                                        //15. Email
                                        //16. Khu vực
                                        model.SaleOfficeCode = item["VKBUR"].ToString();
                                        //17. Địa chỉ
                                        model.Address = item["ADDRESS"].ToString();
                                        //18. Tỉnh/Thành phố
                                        model.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        model.DistrictId = DistrictId;
                                        //20. Phường/Xã
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái
                                        model.Actived = true;
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty
                                        model.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        model.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy
                                        model.CreateBy = SYSTEM;
                                        //29. Thời gian tạo
                                        model.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        //31. Thời gian tạo
                                        //32. Phân nhóm KH/ Customer Account Group
                                        model.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        model.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        model.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        model.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        model.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        model.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                        model.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            model.TaxNo = TaxNo;
                                        }
                                        //nguồn KH
                                        model.CustomerSourceCode = "SHOWROOM";

                                        _context.Entry(model).State = EntityState.Added;
                                        //thêm nhóm KH
                                        AddProfileGroup(item, CompanyCode, ProfileId);

                                        //thêm NVKD
                                        AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);

                                        //profileCodeList.Add(ProfileForeignCode);
                                        //if (profileCodeList.Contains(ProfileForeignCode))
                                        //{
                                        //    profileList.Add(model);
                                        //}
                                    }
                                    //thêm address book
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    if (isAddressBook == false)
                                    {
                                        //1. GUID
                                        ProfileId = existProfileWithSAPCode.ProfileId;
                                        //2. ProfileCode
                                        if (ProfileCode != 0)
                                        {
                                            existProfileWithSAPCode.ProfileCode = ProfileCode;
                                        }
                                        //3. ProfileForeignCode
                                        existProfileWithSAPCode.ProfileForeignCode = ProfileForeignCode;
                                        //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                        existProfileWithSAPCode.isForeignCustomer = isForeignCustomer;
                                        existProfileWithSAPCode.CountryCode = CountryCode;
                                        //5. Danh xưng
                                        var Title = item["ANRED"].ToString();
                                        if (Title.Length > 10)
                                        {
                                            Title = null;
                                        }
                                        existProfileWithSAPCode.Title = Title;
                                        ////6. Loại
                                        existProfileWithSAPCode.CustomerTypeCode = ConstCustomerType.Account;

                                        //Phân loại KH
                                        var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                        if (existProfileType == null)
                                        {
                                            ProfileTypeModel profileType = new ProfileTypeModel();
                                            profileType.ProfileTypeId = Guid.NewGuid();
                                            profileType.ProfileId = ProfileId;
                                            //if (existProfileWithSAPCode.Title == "Company")
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                            //}
                                            //else
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                            //}
                                            profileType.CustomerTypeCode = ProfileTypeCode;
                                            profileType.CompanyCode = item["BUKRS"].ToString();
                                            profileType.CreateBy = SYSTEM;
                                            profileType.CreateTime = CreateTime;
                                            _context.Entry(profileType).State = EntityState.Added;
                                            //typeList.Add(profileType);
                                        }
                                        else
                                        {
                                            existProfileType.CustomerTypeCode = ProfileTypeCode;
                                            _context.Entry(existProfileType).State = EntityState.Modified;
                                        }
                                        //7. Họ va Tên|Tên công ty
                                        existProfileWithSAPCode.ProfileName = item["FULLNAME"].ToString();
                                        //8. Tên ngắn
                                        existProfileWithSAPCode.ProfileShortName = item["NAME1"].ToString();
                                        //9. Tên viết tắt
                                        existProfileWithSAPCode.AbbreviatedName = existProfileWithSAPCode.ProfileName.ToAbbreviation();
                                        //10. Ngày sinh
                                        //11. Tháng sinh
                                        //12. Năm sinh
                                        //13. Độ tuổi
                                        //14. Số điện thoại
                                        try
                                        {
                                            //Delete all
                                            var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                            if (phoneExistLst != null && phoneExistLst.Count > 0)
                                            {
                                                _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                            }
                                            //Add again
                                            if (!string.IsNullOrEmpty(Phone))
                                            {
                                                if (Phone.Contains("-"))
                                                {
                                                    var arr = Phone.Split('-').ToList();
                                                    var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                                    var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                    foreach (var phoneItem in phoneArray)
                                                    {
                                                        ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                        phoneModel.PhoneId = Guid.NewGuid();
                                                        phoneModel.ProfileId = ProfileId;
                                                        phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                        if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                        {
                                                            _context.Entry(phoneModel).State = EntityState.Added;
                                                            //phoneList.Add(phoneModel);
                                                        }
                                                    }
                                                    existProfileWithSAPCode.Phone = PhoneNumber.Trim();
                                                }
                                                else
                                                {
                                                    existProfileWithSAPCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                                }
                                            }
                                            else
                                            {
                                                existProfileWithSAPCode.Phone = null;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // ghi log
                                            string mess = ex.Message;
                                            if (ex.InnerException != null)
                                            {
                                                mess = ex.InnerException.Message;
                                                if (ex.InnerException.InnerException != null)
                                                {
                                                    mess = ex.InnerException.InnerException.Message;
                                                }
                                            }
                                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                        }
                                        //15. Email
                                        //16. Khu vực
                                        existProfileWithSAPCode.SaleOfficeCode = item["VKBUR"].ToString();
                                        //17. Địa chỉ
                                        existProfileWithSAPCode.Address = item["ADDRESS"].ToString();
                                        //18. Tỉnh/Thành phố
                                        existProfileWithSAPCode.ProvinceId = ProvinceId;
                                        //19. Quận/Huyện
                                        existProfileWithSAPCode.DistrictId = DistrictId;
                                        //20. Phường/Xã
                                        //21. Ghi chú
                                        //22. Ngày ghé thăm
                                        //23. Trạng thái (not update this field)
                                        //24. Hình ảnh
                                        //25. Nhân viên tạo
                                        //26. Tạo tại công ty 
                                        existProfileWithSAPCode.CreateAtCompany = item["BUKRS"].ToString();
                                        //27. Tạo tại cửa hàng
                                        existProfileWithSAPCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                        //28. CreateBy (not update this field)
                                        //29. Thời gian tạo
                                        //exist.CreateTime = CreateTime;
                                        //30. LastEditBy
                                        existProfileWithSAPCode.LastEditBy = SYSTEM;
                                        //31. Thời gian sửa
                                        existProfileWithSAPCode.LastEditTime = DateTime.Now;
                                        //32. Phân nhóm KH/ Customer Account Group
                                        existProfileWithSAPCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                        //33.Mã nhóm  KH/ Customer Group
                                        existProfileWithSAPCode.CustomerGroupCode = item["KDGRP"].ToString();
                                        //34. Mã Điều khoản thanh toán/ Payment Term
                                        existProfileWithSAPCode.PaymentTermCode = item["ZTERM"].ToString();
                                        //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                        existProfileWithSAPCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                        //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                        existProfileWithSAPCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                        //37. Mã tài khoản công nợ/ Reconcile Account
                                        existProfileWithSAPCode.ReconcileAccountCode = item["AKONT"].ToString();
                                        //38. Số điện thoại (SAP)
                                        existProfileWithSAPCode.SAPPhone = Phone;
                                        //39. Mã số thuế TaxNo
                                        if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                        {
                                            existProfileWithSAPCode.TaxNo = TaxNo;
                                        }
                                        existProfileWithSAPCode.CustomerSourceCode = "SHOWROOM";
                                        if (!string.IsNullOrEmpty(existProfileWithSAPCode.Email) && IsValidEmail(existProfileWithSAPCode.Email) == false)
                                        {
                                            existProfileWithSAPCode.Note = string.Format("(Email: {0})", existProfileWithSAPCode.Email);
                                            existProfileWithSAPCode.Email = null;
                                        }
                                        _context.Entry(existProfileWithSAPCode).State = EntityState.Modified;
                                        //profileCodeList.Add(ProfileForeignCode);
                                    }
                                    //cập nhật address book
                                    else
                                    {

                                    }
                                }
                            }
                            #endregion

                            #region ProfileBAttributeModel: TaxNo
                            //Delete all
                            //var existAttr = _context.ProfileBAttributeModel
                            //                        .FirstOrDefault(p => p.ProfileId == ProfileId);
                            //if (existAttr != null)
                            //{
                            //    _context.Entry(existAttr).State = EntityState.Deleted;
                            //}
                            ////Add again
                            //ProfileBAttributeModel attr = new ProfileBAttributeModel();
                            //attr.ProfileId = ProfileId;
                            //if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                            //{
                            //    attr.TaxNo = TaxNo;
                            //}
                            //_context.Entry(attr).State = EntityState.Added;
                            //if (!profileCodeList.Contains(ProfileForeignCode))
                            //{
                            //    bAttrList.Add(attr);
                            //}
                            #endregion

                            #region RoleInChargeModel
                            //var roleCharge = _context.RoleInChargeModel
                            //                         .FirstOrDefault(p => p.ProfileId == ProfileId && p.CreateBy == SYSTEM);
                            //if (roleCharge != null)
                            //{
                            //    _context.Entry(roleCharge).State = EntityState.Deleted;
                            //}

                            //var RolesCode = item["ZP_DEPT"].ToString();
                            //if (!string.IsNullOrEmpty(RolesCode) && RolesCode != "00000000")
                            //{
                            //    var role = _context.RolesModel.Where(p => p.RolesCode == RolesCode).FirstOrDefault();
                            //    if (role != null)
                            //    {
                            //        RoleInChargeModel roleModel = new RoleInChargeModel();
                            //        roleModel.RoleInChargeId = Guid.NewGuid();
                            //        roleModel.ProfileId = ProfileId;
                            //        roleModel.RolesId = role.RolesId;
                            //        roleModel.CreateBy = SYSTEM;
                            //        roleModel.CreateTime = CreateTime;
                            //        _context.Entry(roleModel).State = EntityState.Added;
                            //        //if (profileCodeList.Contains(ProfileForeignCode))
                            //        //{
                            //        //    roleList.Add(roleModel);
                            //        //}
                            //    }
                            //}
                            #endregion

                            #region AddressBookModel
                            //if (isContact == true)
                            //{
                            //    var existAddress = _context.AddressBookModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                            //    if (existAddress != null)
                            //    {
                            //        _context.Entry(existAddress).State = EntityState.Deleted;
                            //    }

                            //    AddressBookModel address = new AddressBookModel();
                            //    address.AddressBookId = Guid.NewGuid();
                            //    address.ProfileId = ProfileId;
                            //    address.Address = item["ADDRESS"].ToString();
                            //    address.ProvinceId = ProvinceId;
                            //    address.DistrictId = DistrictId;
                            //    address.CountryCode = ConstCountry.VN;
                            //    address.AddressTypeCode = ConstAddressType.GH;
                            //    address.CreateBy = SYSTEM;
                            //    address.CreateTime = DateTime.Now;
                            //    //_context.Entry(address).State = EntityState.Added;
                            //    addressList.Add(address);
                            //}
                            #endregion

                            #region ProfileContactAttributeModel
                            //if (isContact == true)
                            //{
                            //    var existProfileCode = ProfileForeignCode.Substring(0, 8);
                            //    var existProfile = _context.ProfileModel.FirstOrDefault(p => p.ProfileForeignCode == existProfileCode);
                            //    if (existProfile != null)
                            //    {
                            //        var existContact = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                            //        if (existContact != null)
                            //        {
                            //            _context.Entry(existContact).State = EntityState.Deleted;
                            //        }

                            //        ProfileContactAttributeModel contactAttr = new ProfileContactAttributeModel();
                            //        contactAttr.ProfileId = ProfileId;
                            //        contactAttr.CompanyId = existProfile.ProfileId;
                            //        //_context.Entry(contactAttr).State = EntityState.Added;
                            //        contactList.Add(contactAttr);
                            //    }
                            //}
                            #endregion

                            _context.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors (ProfileSAPCode: {2}): ",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State, item["KUNNR"].ToString()));
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    if (ve.PropertyName == "Email")
                                    {
                                        //var ProfileForeignCode = item["KUNNR"].ToString();
                                        //var profile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                        //if (profile != null && !string.IsNullOrEmpty(profile.Email))
                                        //{
                                        //    profile.Note = string.Format("(Email: {0})", profile.Email);
                                        //    profile.Email = null;
                                        //    _context.Entry(profile).State = EntityState.Modified;
                                        //    _context.SaveChanges();

                                        //    WriteLogFile(logFilePath, "Update Email => Note");
                                        //}
                                    }
                                    else
                                    {
                                        WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var mess = ex.Message;
                            if (ex.InnerException != null)
                            {
                                if (ex.InnerException.InnerException != null)
                                {
                                    mess = ex.InnerException.InnerException.Message;
                                }
                                else
                                {
                                    mess = ex.InnerException.Message;
                                }
                            }
                            WriteLogFile(logFilePath, "Sync Data error: " + mess);
                            continue;
                        }
                    }
                    //if (profileList != null && profileList.Count > 0)
                    //{
                    //    _context.ProfileModel.AddRange(profileList);
                    //}
                    //_context.ProfilePhoneModel.AddRange(phoneList);
                    //_context.ProfileBAttributeModel.AddRange(bAttrList);
                    //_context.PersonInChargeModel.AddRange(personList);
                    //_context.RoleInChargeModel.AddRange(roleList);
                    //_context.ProfileContactAttributeModel.AddRange(contactList);
                    //_context.ProfileTypeModel.AddRange(typeList);
                    //_context.ProfileGroupModel.AddRange(groupList);

                    //_context.ChangeTracker.DetectChanges();
                    //_context.SaveChanges();

                    //MessageBox.Show("Đã đồng bộ thành công!");

                    WriteLogFile(logFilePath, "SYNC PROFILE: Stop.");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var mess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            mess = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            mess = ex.InnerException.Message;
                        }
                    }
                    //ghi log
                    //MessageBox.Show(mess);
                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                }
                //}
                //MessageBox.Show("Đã đồng bộ: " + dt.Rows.Count + " dòng.");
            }
        }
        #endregion

        #endregion

    }
}
