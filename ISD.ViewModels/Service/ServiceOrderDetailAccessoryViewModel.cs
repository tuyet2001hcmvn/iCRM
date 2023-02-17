using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Service
{
    //public class ServiceOrderDetailAccessoryViewModel
    //{
    //    public System.Guid ServiceOrderDetailAccessoryId { get; set; }
    //    public Nullable<System.Guid> ServiceOrderId { get; set; }
    //    public string MaterialNumber { get; set; }
    //    public string ShortText { get; set; }
    //    public string UOM { get; set; }
    //    public Nullable<decimal> Price { get; set; }
    //    public Nullable<int> Quantity { get; set; }
    //    public Nullable<decimal> Total { get; set; }
    //}

    public class ServiceOrderDetailAccessoryViewModel
    {
        

        public int STT { get; set; }
        public System.Guid? ServiceOrderDetailAccessoryId { get; set; }
        public System.Guid? ServiceOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCode")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryName")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public int? Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DetailUnitPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? UnitPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock")]
        public int Stock { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockPosition")]
        public string StockPosition { get; set; }
        [Display(Name = "Bảo hành khẩn")]
        public bool? UrgentServiceOrder { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? TotalPrice { get; set; }

        [Display(Name = "Mã kho")]
        public string WarehouseCode { get; set; }
        [Display(Name = "Vị trí lưu kho")]
        public string Location { get; set; }

        [Display(Name = "Tiền công")]
        public Nullable<decimal> AccessoryPrice { get; set; }

        [Display(Name = "Loại sửa chữa")]
        //public string ServiceTypeCode { get; set; }
        public Guid? FixingTypeId { get; set; }

        [Display(Name = "Loại sửa chữa")]
        public string FixingTypeName { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Tên kho")]
        public string WarehouseName { get; set; }

        public int? Number { get; set; }
    }
}