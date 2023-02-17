using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class OnHandMaterialViewModel
    {
        //MANDT
        public string Mandt { get; set; }

        //Công ty
        //WERKS
        public string CompanyCode { get; set; }

        //Mã sản phẩm
        //MATNR
        public string MaterialCode { get; set; }

        //Mã kho
        //LGORT
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public string WarehouseCode { get; set; }

        //Batch
        //CHARG
        public string Batch { get; set; }

        //Số lượng
        //MENGE
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public decimal? Quantity { get; set; }

        //Đơn vị tính
        //MEINS
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        //Ngày nhập khẩu
        //NGAY_NK
        public DateTime? IncomeDate { get; set; }

        //Ngày sản xuất
        //NGAY_XK
        public DateTime? ProduceDate { get; set; }

        //Số khung
        //SOKHUNG
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string ChassisNumber { get; set; }

        //Số máy
        //SOMAY
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        //Ngày nhập mua
        //NGAY_NHAPMUA
        public DateTime? BuyDate { get; set; }

        //Đăng kiểm
        //DANGKIEM
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_IsGroup")]
        public bool? IsGroup { get; set; }

        //Tên tạm (Đã báo cáo nhưng chưa bán thực tế)
        //TENTAM
        public string TempName { get; set; }

        //Ghi chú 1
        //GHICHU1
        public string Note1 { get; set; }

        //Ghi chú 2
        //GHICHU2
        public string Note2 { get; set; }

        //Ghi chú 3
        //GHICHU3
        public string Note3 { get; set; }

        //Tên kho
        //LGOBE
        public string WarehouseName { get; set; }

    }
}
