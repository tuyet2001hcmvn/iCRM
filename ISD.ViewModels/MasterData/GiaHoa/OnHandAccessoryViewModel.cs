using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class OnHandAccessoryViewModel
    {
        //MANDT
        public string Mandt { get; set; }

        //Công ty
        //WERKS
        public string CompanyCode { get; set; }

        //Mã kho
        //LGORT
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public string WarehouseCode { get; set; }

        //Mã phụ tùng/phụ kiện/công việc
        //MATNR
        public string AccessoryCode { get; set; }

        //Số lượng
        //MENGE
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public decimal? Quantity { get; set; }

        //Đơn vị tính
        //MEINS
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        //Vị trí (Bin)
        //LGPLA
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Location")]
        public string Location { get; set; }

        //Special Stock
        //SOBKZ
        public string SpecialStock { get; set; }

        //Special Stock Number
        //SONUM
        public string SpecialStockNumber { get; set; }

        //PO number
        public string PONumber { get; set; }

        //ServiceOrderId (text then transfer to Guid)
        //BSTKD
        public string ServiceOrderId { get; set; }

        //Tên kho
        //LGOBE
        [Display(Name = "Tên kho")]
        public string WarehouseName { get; set; }

    }
}
