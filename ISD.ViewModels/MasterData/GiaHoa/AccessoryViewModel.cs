using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class AccessoryViewModel
    {
        public Guid AccessoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryCode")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryType")]
        public string AccessoryType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string AccessoryUnit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string AccessoryUnitName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategoryType")]
        public int AccessoryByCategoryType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceType")]
        public string ServiceTypeCode { get; set; }

        //Mã plant
        //PLANT
        public string Plant { get; set; }

        //Mã kho
        //LGORT
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public string WarehouseCode { get; set; }

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

        //Giá
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        public decimal? AccessoryPrice { get; set; }

        //Số lượng người dùng nhập để chuyển kho và thêm vào lưới
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InputQuantity")]
        public int? InputQuantity { get; set; }

        //Mã khuyến mãi
        public Guid? SearchPromotionId { get; set; }

        //Mã kho
        //LGORT
        [Display(Name = "Tên kho")]
        public string WarehouseName { get; set; }
    }

    //Giá phụ kiện
    public class AccessoryPriceViewModel
    {
        public Guid AccessoryPriceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConditionType")]
        public string ConditionType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PriceGroup")]
        public string PriceGroup { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_UnitPrice")]
        public decimal? UnitPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string UOM { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedFromDate")]
        public DateTime? EffectedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedToDate")]
        public DateTime? EffectedToDate { get; set; }

        public string SAPCode { get; set; }

        public bool? Actived { get; set; }
    }
}
