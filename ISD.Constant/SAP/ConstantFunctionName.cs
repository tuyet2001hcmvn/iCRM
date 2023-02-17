using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public class ConstantFunctionName
    {
        //1. Customer
        public const string YAC_FM_CRM_GET_DATALIST = "YAC_FM_CRM_GET_DATALIST";

        //2. Credit Limit
        public const string YAC_FM_CRM_GET_LEVEL_DEBIT = "YAC_FM_CRM_GET_LEVEL_DEBIT";

        //3. Material
        public const string YAC_FM_CRM_GET_MATERIAL = "YAC_FM_CRM_GET_MATERIAL";

        //4. Revenue
        public const string YAC_FM_WEB_GET_DOANHSO = "YAC_FM_WEB_GET_DOANHSO";

        //5.Check serial status
        public const string YAC_FM_CRM_CHECK_SERIAL_BH = "YAC_FM_CRM_CHECK_SERIAL_BH";

        //6. Check OD information
        public const string YAC_FM_CRM_GET_OD_BH = "YAC_FM_CRM_GET_OD_BH";

        //Giải mã password
        public const string YAC_FM_GET_GIAIMA = "YAC_FM_GET_GIAIMA";

        //Trả về danh sách SO-Đơn hàng theo Customer
        public const string YAC_FM_CRM_GET_SO_CUST = "YAC_FM_CRM_GET_SO_CUST";

        //Trả về doanh số theo từng tháng của Customer
        public const string YAC_FM_CRM_GET_DOANHSO = "YAC_FM_CRM_GET_DOANHSO";

        //Trả về các danh mục cơ bản để duyệt hạn mức
        public const string YAC_FM_CRM_GET_CREDITLIST = "YAC_FM_CRM_GET_CREDITLIST";

        //Trả về thông tin hạn mức công nợ khách hàng
        public const string YAC_FM_CRM_GET_CREDITLIMIT = "YAC_FM_CRM_GET_CREDITLIMIT";

        //Trả về danh sách user login
        public const string YAC_FM_CRM_LIST_USER = "YAC_FM_CRM_LIST_USER";

        //Trả về thông tin user login
        public const string YAC_FM_CRM_CHECK_LOGIN = "YAC_FM_CRM_CHECK_LOGIN";

        //Set trạng thái Duyệt/Bỏ Duyệt hạn mức khách hàng vào SAP Table
        //public const string YAC_FM_CRM_SET_CREDITLIMIT = "YAC_FM_CRM_SET_CREDITLIMIT";
        public const string YAC_FM_CRM_SET_CREDITLIMIT = "YAC_FM_CRM_SET_CREDITLIMIT_V2";

        //Trả về các danh mục cơ ban
        public const string YAC_FM_CRM_GET_DANHMUC = "YAC_FM_CRM_GET_DANHMUC";

        //Trả về thông tin doanh số mua và thanh toán(5.1 Profile)
        public const string YAC_FM_CRM_GET_CONGNO_KH = "YAC_FM_CRM_GET_CONGNO_KH";

        //Trả về thông tin doanh số chi tiết mua hang (5.2 Profile)
        public const string YAC_FM_CRM_GET_MUAHANG = "YAC_FM_CRM_GET_MUAHANG";

        //Trả về thông tin phân cấp doanh số theo nhân viên phòng ban (5.3 - 4.2.22)
        public const string YAC_FM_CRM_GET_PHANCAP_DSO = "YAC_FM_CRM_GET_PHANCAP_DSO";

        //Trả về thông tin phân cấp doanh số theo nhân viên phòng ban (5.4 - 4.2.24)
        public const string YAC_FM_CRM_GET_TANGTRUONG_DSO = "YAC_FM_CRM_GET_TANGTRUONG_DSO";

        //Trả về thông tin chi tiết doanh số khách hàng (5.5 - 4.2.25)
        public const string YAC_FM_CRM_GET_DOANHSO_KHACH = "YAC_FM_CRM_GET_DOANHSO_KHACH";

        //Trả về giá trị của các SO (5.6)
        public const string YAC_FM_CRM_GET_VALUE_SO = "YAC_FM_CRM_GET_VALUE_SO";

        //Trả về hạng mục mô tả đánh giá khách hàng
        public const string YAC_FM_CRM_GET_DANH_GIA = "YAC_FM_CRM_GET_DANH_GIA";
    }
}
