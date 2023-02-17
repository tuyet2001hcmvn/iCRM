using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public static class ConstRoles
    {
        public const int isSysAdmin = 0;

        //Khách hàng
        public const string Customer = "CUSTOMER";

        //Nhân viên bán hàng
        public const string Sale = "SALE";
        //Nhân viên kiểm tra xe trước khi rời khỏi CH
        public const string NVKiemTra = "NVKIEMTRA";
        //Nhân viên dịch vụ (kỹ thuật viên)
        public const string Service = "SERVICE";
        //Kỹ thuật trưởng
        public const string KTT = "KTT";
        //Báo cáo
        public const string BAOCAO = "BAOCAO";

        //Thủ kho
        public const string THUKHO = "THUKHO";

        //Thu ngân
        public const string THUNGAN = "THUNGAN";

        //Phụ tùng
        public const string PHUTUNG = "PHUTUNG";

        //Tiếp nhận
        public const string TIEPNHAN = "TIEPNHAN";

        //Kế toán
        public const string KETOAN = "KETOAN";
    }
    public static class ConstRolesForTienThuApp
    {
        #region - Chưa Sử dụng
        ////Web - Admin
        //public static Guid ADMIN = new Guid ("A8C06D08-16DF-4188-9515-58C39D393E08");
        ////SysAdmin - Developer
        //public static Guid SYSADMIN = new Guid("43A65EB8-5A75-4235-8CA8-92C091823642");
        ////Mobile - Bán hàng
        //public static Guid SALE = new Guid("758F03C6-01C4-45B0-8D10-A20E2331150E");
        ////Mobile - Dịch vụ
        //public static Guid SERVICE = new Guid("D4304F0A-067F-4D67-A40C-0B5666A01018");
        #endregion

        //Mobile - Khách hàng
        public static Guid CUSTOMER = new Guid("60B32D4F-EE5D-47E7-99A2-6324F1229C59");
    }
}
