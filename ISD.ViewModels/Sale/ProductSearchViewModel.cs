using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProductSearchViewModel
    {
        public System.Guid ProductId { get; set; }
        
        //Product details
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string SearchProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string SearchERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string SearchProductName { get; set; }

        //Brand
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public System.Guid? SearchBrandId { get; set; }

        //Parent Category
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public System.Guid? SearchParentCategoryId { get; set; }

        //Category
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public System.Guid? SearchCategoryId { get; set; }

        //Configuration
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        public System.Guid? SearchConfigurationId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_isHot")]
        public Nullable<bool> isHot { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }




        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]

        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]

        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductGroups")]
        public string ProductGroups { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]

        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]

        public Guid? BrandId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]

        public Guid? CategoryId { get; set; }
        
        public Guid? ConfigurationId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]

        public Guid? ParentCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SerialNumber")]
        public string SerriNo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SearchWithWarranty")]
        public bool SearchWithWarranty { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
