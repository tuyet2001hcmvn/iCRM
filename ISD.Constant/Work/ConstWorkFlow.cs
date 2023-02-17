using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public class ConstWorkFlow
    {
        //1. Ghé thăm
        public static string GT = "GT";

        //2. Khảo sát
        public static string KS = "KS";

        //3. Lắp đặt
        public static string LĐ = "LĐ";

        //4. Bảo hành
        public static string BaoHanh = "BH";
        public static string BH_AnCuong = "BH_ACC";
        public static string BH_Malloca = "BH_MLC";
        public static string BH_Aconcept = "BH_ACT";

        //5. Thăm hỏi khách hàng
        public static string THKH = "THKH";

        //5. Góc vật liệu
        public static string GVL = "GVL";
        public static Guid GocVatLieu = Guid.Parse("dfd830ef-4db4-420c-b9d9-9e094bb07760");
        public static Guid GocA5 = Guid.Parse("ee3aad39-a016-41f7-9b10-16f3bd1dabd3");
        public static Guid KeVanSan = Guid.Parse("6e49403a-f439-4638-8005-daac978e6ec6");
        public static Guid BucPK = Guid.Parse("71ff33ec-b238-4e83-bdd4-df8e6ebeec56");
        public static Guid VachPhoi = Guid.Parse("6799cf65-8537-4df9-872e-9b6f98dce313");

        //6. CSKH ĐB
        public static string CSKH = "CSKH";

        //7. Nhiệm vụ
        public static string NV = "NV";

        //8. Hướng dẫn sử dụng
        public static string HDSD = "HDSD";

        //9. Gửi thư điện tử
        public static string GTĐT = "GTĐT";

        //10. Gọi điện
        public static string GĐ = "GĐ";

        /// <summary>
        /// Id của workfow góc vật liệu
        /// </summary>
        public static Guid GVLID = Guid.Parse("dfd830ef-4db4-420c-b9d9-9e094bb07760");

        //Hoạt động dự án
        public static string PROJECT = "PROJECT";
    }
}
