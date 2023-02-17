using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class DeliveryDetailViewModel
    {
        public int STT { get; set; }
        public System.Guid DeliveryDetailId { get; set; }
        public Nullable<System.Guid> DeliveryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public Nullable<System.Guid> ProductId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        public Nullable<System.Guid> StockId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockCode")]
        public string StockCode { get; set; }
        public string StockName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        public Nullable<int> DateKey { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        [DisplayFormat(DataFormatString = "{0:#}")]
        public Nullable<decimal> Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        public Nullable<decimal> UnitPrice
        {
            get
            {
                return Price * Quantity;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string DetailNote { get; set; }

        public Nullable<bool> isDeleted { get; set; }

        public int? ProductQuantinyOnHand { get; set; }
    }
}