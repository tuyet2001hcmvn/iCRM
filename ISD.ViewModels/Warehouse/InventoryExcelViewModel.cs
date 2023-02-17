using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class InventoryExcelViewModel
    {
        public Guid? StockReceivingId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }

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
        public string Note { get; set; }
        public string CompanyCode { get; set; }
        public string StoreCode { get; set; }
        public DateTime DocumentDate { get; set; }
        public int ProfileCode { get; set; }
        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }

    public class InventoryMasterExcellViewModel
    {
        public int Index { get; set; }
        public string Value { get; set; }
    }
}