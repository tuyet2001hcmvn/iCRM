using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class StockReceivingDetailViewModel
    {
        //Detail
        public int? STT { get; set; }

        public System.Guid StockReceivingDetailId { get; set; }

        public Nullable<System.Guid> StockReceivingId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> StockId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockCode")]
        public string StockCode { get; set; }

        public string StockName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        public Nullable<int> DateKey { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DisplayFormat(DataFormatString = "{0:#}")]
        public Nullable<decimal> Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
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
    }
}