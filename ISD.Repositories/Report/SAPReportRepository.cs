using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Report
{
    public class SAPReportRepository
    {
        //SAPRepository _sap;
        //RfcDestination destination;
        EntityDataContext _context;
        public SAPReportRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<CustomerSaleOrderViewModel> GetSaleOrderList(string ProfileForeignCode, string CompanyCode)
        {
            var listResult = new List<CustomerSaleOrderViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_SO_CUST);
            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_KUNNR", ProfileForeignCode); //Mã SAP Khách hàng

            function.Invoke(destination);

            var datatable = function.GetTable("SALEORDER_T").ToDataTable("SALEORDER_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                foreach (DataRow item in datatable.Rows)
                {

                    listResult.Add(new CustomerSaleOrderViewModel()
                    {
                        SONumber = item["VBELN"].ToString(),//Mã đơn hàng
                        OrderNumber = item["VBELN_OD"].ToString(),//Mã lệnh
                        ProductCode = item["MATNR"].ToString().TrimStart(new Char[] { '0' }),//Mã sản phẩm
                        ProductName = item["MAKTX"].ToString(),//Tên sản phẩm
                        ProductQuantity = (decimal?)item["KWMENG"],//Số lượng
                    });
                }
            }

            return listResult;
        }



        /// <summary>
        /// YAC_FM_CRM_GET_DANHMUC | Trả về các danh mục cơ ban
        /// </summary>
        /// <param name="IM_WERKS">Mã công ty</param>
        /// <param name="IM_TYPE"> Loại danh muc: 
        ///                             1.Phòng ban
        ///                             2.Nhân viên
        ///                             3.Khu vực
        ///                             4.Tỉnh-Thành
        ///                             5.Quận-Huyện
        ///                             6.Nhóm khách hàng
        ///                             7.Ngành nghề
        ///                             8.Nhóm doanh số
        /// </param>
        /// <returns></returns>
        public List<SAPGetDanhMucViewModel> GetDanhMuc(string CompanyCode, string Type)
        {
            var listResult = new List<SAPGetDanhMucViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DANHMUC);
            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_TYPE", Type); //Loại danh mục

            function.Invoke(destination);

            var datatable = function.GetTable("LIST_T").ToDataTable("LIST_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPGetDanhMucViewModel()
                    {
                        Type = item["TYPENO"].ToString(),//Mã đơn hàng
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        Code = item["CODENO"].ToString(),//Mã dữ liệu
                        Description = item["DATANM"].ToString(),//Mô tả dữ liệu
                        Content = item["LONGDATA"].ToString(),//Mô tả dữ liệu dài
                        FromValue = !string.IsNullOrEmpty(item["FRVALUE"].ToString()) ? Convert.ToDecimal(item["FRVALUE"]) : 0,
                        ToValue = !string.IsNullOrEmpty(item["TOVALUE"].ToString()) ? Convert.ToDecimal(item["TOVALUE"]) : 0,
                    });
                }
            }

            return listResult;
        }

        /// <summary>
        /// YAC_FM_CRM_GET_CONGNO_KH | Trả về thông tin doanh số mua và thanh toán(5.1 Profile)
        /// </summary>
        /// <param name="CompanyCode">Mã công ty. Truyền '%' là lấy 2 công ty 1000 & 4000 cộng lại. Ngược lại truyền theo từng công ty thì chỉ tính công ty đó thôi</param>
        /// <param name="Year">Năm</param>
        /// <param name="ProfileForeignCode">Mã SAP Khách hàng</param>
        /// <returns></returns>
        public List<SAPGetCongNoProfileViewModel> GetCongNoProfile(string CompanyCode, string Year, string ProfileForeignCode)
        {
            var listResult = new List<SAPGetCongNoProfileViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_CONGNO_KH);
            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_NAM", Year); //Năm
            function.SetValue("IM_KUNNR", ProfileForeignCode); //Mã SAP khách hàng

            function.Invoke(destination);

            var datatable = function.GetTable("CREDIT_T").ToDataTable("CREDIT_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPGetCongNoProfileViewModel()
                    {
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        Year = item["NAM"].ToString(),//Năm
                        ProfileCode = item["KUNNR"].ToString(),//Mã khách hàng
                        DoanhSo = (decimal?)item["DOANHSO"],//Doanh số
                        ThanhToan = (decimal?)item["THANHTOAN"],
                        ConLai = (decimal?)item["CONLAI"],
                        DuDauKy = (decimal?)item["DUDAUKY"],
                        NoTrongKy = (decimal?)item["INKYNO"],
                        CoTrongKy = (decimal?)item["INKYCO"],
                        DuCuoiKy = (decimal?)item["DUCUOIKY"]
                    });
                }
            }
            return listResult;
        }
        /// <summary>
        /// YAC_FM_CRM_GET_MUAHANG | Trả về thông tin doanh số chi tiết mua hang (5.2 Profile)
        /// </summary>
        /// <param name="CompanyCode">Mã công ty. Truyền '%' là lấy 2 công ty 1000 & 4000 cộng lại. Ngược lại truyền theo từng công ty thì chỉ tính công ty đó thôi</param>
        /// <param name="Year">Năm</param>
        /// <param name="ProfileForeignCode">Mã SAP Khách hàng</param>
        /// <returns></returns>
        public List<SAPGetMuaHangProfileViewModel> GetMuaHangProfile(string CompanyCode, string Year, string ProfileForeignCode)
        {
            var listResult = new List<SAPGetMuaHangProfileViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_MUAHANG);
            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_NAM", Year); //Năm
            function.SetValue("IM_KUNNR", ProfileForeignCode); //Mã SAP khách hàng

            function.Invoke(destination);

            var datatable = function.GetTable("DOANHSO_T").ToDataTable("DOANHSO_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPGetMuaHangProfileViewModel()
                    {
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        ProfileForeignCode = item["KUNNR"].ToString(),//Mã khách hàng
                        GroupProductCode = item["MATKL"].ToString(),//Mã nhóm sản phẩm
                        GroupProductName = item["MATKLNM"].ToString(),//Tên nhóm sản phẩm
                        Year2 = (decimal?)item["DOANHSO2"],//Doanh số năm IM_NAM - 2
                        Year1 = (decimal?)item["DOANHSO1"],////Doanh số năm IM_NAM - 1
                        Year = (decimal?)item["DOANHSO0"],//Doanh số năm IM_NAM
                        Total = (decimal?)item["TONGDOANHSO"]//Tổng doanh số = DOANHSO1 + DOANHSO2 + DOANHSO3
                    });
                }
            }


            return listResult;
        }

        public List<SAPGetPhanCapDoanhSoViewModel> GetPhanCapDoanhSo(string CompanyCode, string RolesCode, string EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            var listResult = new List<SAPGetPhanCapDoanhSoViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_PHANCAP_DSO);

            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_DEPTNO", RolesCode ?? "%"); //Năm
            function.SetValue("IM_EMPNO", EmployeeCode ?? "%"); //Mã SAP khách hàng
            function.SetValue("IM_FRDATE", FromDate.ToString("yyyyMMdd")); //Mã SAP khách hàng
            function.SetValue("IM_TODATE", ToDate.ToString("yyyyMMdd")); //Mã SAP khách hàng

            function.Invoke(destination);

            var datatable = function.GetTable("DOANHSO_T").ToDataTable("DOANHSO_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                var index = 1;
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPGetPhanCapDoanhSoViewModel()
                    {
                        STT = index++,
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        RolesCode = item["DEPTNO"].ToString(),//Mã phòng ban
                        RolesName = item["DEPTNM"].ToString(),//Tên phòng ban
                        SalesEmployeeCode = item["EMPNO"].ToString(),//Mã nhân viên
                        SalesEmployeeName = item["EMPNM"].ToString(),//Tên nhân viên
                        ColName = item["COLNAME"].ToString(),//Tên cột thông tin
                        ColNo = (int?)item["COLNO"],//Thứ tự cột
                        ColValue = (int?)item["COLVALUE"],//Giá trị cột thông tin
                    });
                }
            }


            return listResult;
        }

        public List<SAPGetTangTruongDoanhSoViewModel> GetTangTruongDoanhSo(string CompanyCode, string RolesCode, string EmployeeCode, DateTime FromDate, DateTime ToDate, DateTime PreviousFromDate, DateTime PreviousToDate)
        {
            var listResult = new List<SAPGetTangTruongDoanhSoViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_TANGTRUONG_DSO);

            //Truyền parameters
            function.SetValue("IM_WERKS", CompanyCode); //Mã công ty
            function.SetValue("IM_DEPTNO", RolesCode ?? "%"); //Năm
            function.SetValue("IM_EMPNO", EmployeeCode ?? "%"); //Mã SAP khách hàng
            function.SetValue("IM_FRDATE1", PreviousFromDate.ToString("yyyyMMdd")); //Từ ngày kỳ trước  truyền theo format: yyyymmdd
            function.SetValue("IM_TODATE1", PreviousToDate.ToString("yyyyMMdd")); //Đến ngày kỳ trước truyền theo format: yyyymmdd
            function.SetValue("IM_FRDATE2", FromDate.ToString("yyyyMMdd")); //Từ ngày kỳ này truyền theo format: yyyymmdd
            function.SetValue("IM_TODATE2", ToDate.ToString("yyyyMMdd")); //Đến ngày kỳ này truyền theo format: yyyymmdd
            function.Invoke(destination);

            var datatable = function.GetTable("DOANHSO_T").ToDataTable("DOANHSO_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                var index = 1;
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPGetTangTruongDoanhSoViewModel()
                    {
                        STT = index++,
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        RolesCode = item["DEPTNO"].ToString(),//Mã phòng ban
                        RolesName = item["DEPTNM"].ToString(),//Tên phòng ban
                        SalesEmployeeCode = item["EMPNO"].ToString(),//Mã nhân viên
                        SalesEmployeeName = item["EMPNM"].ToString(),//Tên nhân viên
                        //ProfileCode = ,//Mã khách hàng
                        ProfileForeignCode = item["KUNNR"].ToString(),//Mã SAP
                        ProfileName = item["CUSTNM"].ToString(),//Tên khách hàng
                        Value = string.Format("{0:n1} %", (decimal?)item["TYLEVALUE"]),//Tỷ lệ
                        DoanhSo1 = (decimal?)item["DOANHSO1"],//Tên khách hàng
                        DoanhSo2 = (decimal?)item["DOANHSO2"],//Tên khách hàng
                    });
                }
            }


            return listResult;
        }


        /// <summary>
        /// Trả về giá trị của các SO (5.6)
        /// </summary>
        /// <param name="SO">IM_LISTSO: Chuỗi các SO cách nhau bằng dấu chấm phẩy (;). Ví dụ: SO1;SO2;SO3</param>
        /// <returns></returns>
        public List<SAPValueSOViewModel> GetValueSO(string SO)
        {
            var listResult = new List<SAPValueSOViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_VALUE_SO);

            //Truyền parameters
            function.SetValue("IM_LISTSO", SO); // Số SO
            function.Invoke(destination);

            var datatable = function.GetTable("VALUE_T").ToDataTable("VALUE_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                var index = 1;
                foreach (DataRow item in datatable.Rows)
                {
                    listResult.Add(new SAPValueSOViewModel()
                    {
                        STT = index++,
                        SO = item["LISTSO"].ToString(),//Số SO
                        Value = (decimal?)item["VALUESO"],//Giá trị
                      
                    });
                }
            }


            return listResult;
        }
        public List<SAPGetChiTietDoanhSoViewModel> GetChiTietDoanhSo(CustomerHierarchyDetailReportSearchViewModel viewSearch)
        {
            var listResult = new List<SAPGetChiTietDoanhSoViewModel>();

            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DOANHSO_KHACH);

            //Truyền parameters
            function.SetValue("IM_WERKS", viewSearch.CompanyCode); //Mã công ty
            function.SetValue("IM_DEPTNO", viewSearch.RolesCode != null ? string.Join(";",viewSearch.RolesCode) : "%"); //Phòng ban
            function.SetValue("IM_EMPNO", viewSearch.SalesEmployeeCode != null ? string.Join(";", viewSearch.SalesEmployeeCode) : "%"); //Mã nhân viên
            function.SetValue("IM_KUNNR", viewSearch.ProfileForeignCode != null ? string.Join(";", viewSearch.ProfileForeignCode) : "%"); //Mã khách hàng
            function.SetValue("IM_NHOM", viewSearch.CustomerGroupCode != null ? string.Join(";",viewSearch.CustomerGroupCode) : "%"); //Năm
            function.SetValue("IM_NGANH", viewSearch.CustomerCareerCode != null ? string.Join(";", viewSearch.CustomerCareerCode) : "%"); //Mã SAP khách hàng
            function.SetValue("IM_KHUVUC", viewSearch.SaleOfficeCode != null ? string.Join(";", viewSearch.SaleOfficeCode) : "%"); //Mã SAP khách hàng
            function.SetValue("IM_TINH", viewSearch.ProvinceCode != null ? string.Join(";", viewSearch.ProvinceCode) : "%"); //Mã SAP khách hàng
            function.SetValue("IM_QUAN", viewSearch.DistrictCode != null ? string.Join(";", viewSearch.DistrictCode) : "%"); //Mã SAP khách hàng
            function.SetValue("IM_FRDATE", viewSearch.FromDate.ToString("yyyyMMdd")); //Từ ngày kỳ trước  truyền theo format: yyyymmdd
            function.SetValue("IM_TODATE", viewSearch.ToDate.ToString("yyyyMMdd")); //Đến ngày kỳ trước truyền theo format: yyyymmdd
            function.SetValue("IM_FRDSO", viewSearch.FromValue); //Từ Doanh số
            function.SetValue("IM_TODSO", viewSearch.ToValue); //Đến Doanh số
            //function.SetValue("IM_NHOM_DSO", viewSearch.GroupValueCode != null ? string.Join(";", viewSearch.GroupValueCode) : "%"); //Nhóm doanh số

            function.Invoke(destination);

            var datatable = function.GetTable("DOANHSO_T").ToDataTable("DOANHSO_T");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                var index = 1;
                foreach (DataRow item in datatable.Rows)
                {
                    var profileForeignCode = item["KUNNR"].ToString();
                    var profileCode = _context.ProfileModel.Where(x => x.ProfileForeignCode == profileForeignCode).Select(x => x.ProfileCode).FirstOrDefault();
                    listResult.Add(new SAPGetChiTietDoanhSoViewModel()
                    {
                        STT = index++,
                        CompanyCode = item["WERKS"].ToString(),//Mã công ty
                        RolesCode = item["DEPTNO"].ToString(),//Mã phòng ban
                        RolesName = item["DEPTNM"].ToString(),//Tên phòng ban
                        SalesEmployeeCode = item["EMPNO"].ToString(),//Mã nhân viên
                        SalesEmployeeName = item["EMPNM"].ToString(),//Tên nhân viên
                        ProfileCode = profileCode,//Mã khách hàng
                        ProfileForeignCode = item["KUNNR"].ToString(),//Mã SAP
                        ProfileName = item["CUSTNM"].ToString(),//Tên khách hàng
                        Value = (decimal?)item["DOANHSO"],//Doanh số
                        Address = item["DIACHI"].ToString(),//Địa chỉ
                        CustomerGroupCode = item["KDGRP"].ToString(),//Mã nhóm khách hàng
                        CustomerGroupName = item["KTEXT"].ToString(),//Tên nhóm khách hàng
                        SaleOfficeCode = item["VKBUR"].ToString(),//Mã khu vực
                        SaleOfficeName = item["BEZEI"].ToString(),//Tên khu vực
                        ProvinceCode = item["VKGRP"].ToString(),//Mã tỉnh thành
                        ProvinceName = item["VKGRPNM"].ToString(),//Tên tỉnh thành
                        DistrictCode = item["BZIRK"].ToString(),//Mã quận huyện
                        DistrictName = item["BZTXT"].ToString(),//Tên quận huyện
                        CustomerCareerCode = item["BRSCH"].ToString(),//Mã ngành nghề
                        CustomerCareerName = item["BRTXT"].ToString(),//Tên ngành nghề
                    });
                }
            }


            return listResult;
        }
    }
}
