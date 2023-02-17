using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockReceivingViewModel
    {

        public System.Guid StockReceivingId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceivingCode")]
        public int StockReceivingCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DocumentDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> StoreId { get; set; }
        public string StoreName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SalesEmployeeCode { get; set; }
        public string SalesEmployeeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProfileId")]

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceiving_ProfileCode")]
        public string ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceiving_ProfileName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<bool> isDeleted { get; set; }

        //Detail
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProductId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> StockId { get; set; }
        public string StockName { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        //public Nullable<int> DateKey { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        //public Nullable<decimal> Quantity { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        //public Nullable<decimal> Price { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        //public Nullable<decimal> UnitPrice {
        //    get
        //    {
        //        return Price * Quantity;
        //    }
        //}
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        //public string DetailNote { get; set; }
    }
}
