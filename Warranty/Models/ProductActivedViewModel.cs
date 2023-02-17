using ISD.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Warranty.Models
{
    public class ProductActivedViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductName")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Product_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        public Guid CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Company_CompanyCode")]
        public string CompanyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SaleOrder_Quantity")]
        [DisplayFormat(DataFormatString = "{0:n3}")]
        public decimal? Quantity { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Warranty_ActivationQuantity")]
        public decimal? ActivationQuantity { get; set; }
        public string SaleOrderCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string WarrantyCode { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? PostDate { get; set; }
        public bool isActivedWarranty { get; set; }
    }
}