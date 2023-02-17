using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskProductViewModel
    {
        public Guid? TaskProductId { get; set; }

        public Guid? TaskId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product")]
        public Guid? ProductId { get; set; }

        public string ERPProductCode { get; set; }
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string UnitName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_Quantity")]
        public int? Qty { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Guid? CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Guid? LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public string LastEditByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public DateTime? LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorTypeCode2")]
        public string ErrorTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorCode2")]
        public string ErrorCode { get; set; }
        public string ErrorCodeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductLevelCode")]
        public string ProductLevelCode { get; set; }

        public string ProductLevelName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductColorCode")]
        public string ProductColorCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UsualErrorCode")]
        public string UsualErrorCode { get; set; }

        public string UsualErrorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string ProductCategoryCode { get; set; }

        public string ProductCategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Accessory")]
        public string Accessory { get; set; }

        public List<TaskProductViewModel> accessoryList { get; set; }
        public List<string> usualErrorList { get; set; }

        //Loại phụ kiện
        public string ProductAccessoryTypeCode { get; set; }
        public string ProductAccessoryTypeName { get; set; }
        //Số lượng PK
        public string AccessoryQuantity { get; set; }
        //Tên PK
        public string AccessoryName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_AccErrorTypeCode")]
        public string AccErrorTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_AccErrorTypeCode")]
        public string AccErrorTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SAPSOWarranty")]
        public string SAPSOWarranty { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_Value")]
        public decimal? WarrantyValue { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SAPSOProduct")]
        public string SAPSOProduct { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Value")]
        public decimal? ProductValue { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Discount_Value")]
        public decimal? DiscountValue { get; set; }
        public string SerialNumber { get; set; }
    }
}
