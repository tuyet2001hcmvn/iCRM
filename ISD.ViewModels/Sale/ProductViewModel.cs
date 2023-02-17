using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ProductViewModel
    {
        public System.Guid ProductId { get; set; }
        
        //Product details
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingProductCode", "Product", AdditionalFields = "ProductCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string ProductCode { get; set; }

        public string ProductCodeValid { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ERPProductCode { get; set; }

       
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CylinderCapacity")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n0}")]
        public decimal? CylinderCapacity { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductGroups")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProductGroups { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl", Description = "ImageUrl_RequiredSize")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        ////Brand
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        //public System.Guid BrandId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        //public string BrandName { get; set; }

        //Category
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public System.Guid? ParentCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public string ParentCategoryName { get; set; }

        //Category
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public System.Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string CategoryName { get; set; }

        //Configuration
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        public System.Guid ConfigurationId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        public string ConfigurationName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_GuaranteePeriod")]
        public string GuaranteePeriod { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_isHot")]
        public Nullable<bool> isHot { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public Guid? CompanyId { get; set; }

        //Product Attribute
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Color")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Thickness")]
        public string Thickness { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Allocation")]
        public string Allocation { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_Grade")]
        public string Grade { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Surface")]
        public string Surface { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberOfSurface")]
        public string NumberOfSurface { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GrossWeight")]
        public Nullable<decimal> GrossWeight { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NetWeight")]
        public Nullable<decimal> NetWeight { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WeightUnit")]
        public string WeightUnit { get; set; }

        //Properties
        public System.Guid PropertiesId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Subject")]
        public string Subject { get; set; }

        public string SubjectColor { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Description")]
        //public string Description { get; set; }

        public string DescriptionColor { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Image")]
        public string Image { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_BackgroundColor")]
        public string BackgroundColor { get; set; }

        public string X { get; set; }

        public string Y { get; set; }

        //Specifications
        public System.Guid SpecificationsProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Specifications")]
        public System.Guid SpecificationsId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Specifications")]
        public string SpecificationsName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Description")]
        public string SpecificationsDescription { get; set; }

        //Accessory
        public System.Guid AccessoryProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public System.Guid AccessoryCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public System.Guid AccessoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Price")]
        public decimal? Price { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProcessingValue")]
        public decimal? ProcessingValue { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SampleValue")]
        public decimal? SampleValue { get; set; }

        //Promotion
        public System.Guid PromotionId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
        public string PromotionName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
        public Nullable<System.DateTime> EffectFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
        public Nullable<System.DateTime> EffectToDate { get; set; }

        //Color
        public System.Guid ColorProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_MainColor")]
        public System.Guid? MainColorId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_MainColor")]
        public string MainColorCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorShortName")]
        public string MainColorShortName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_MainColor")]
        public string MainColorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Style")]
        public System.Guid? StyleId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Style")]
        public string StyleName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Image")]
        public string ColorStyleImage { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_isDefault")]
        public bool? isDefault { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_NumberOfImage")]
        public int NumberOfImage { get; set; }

        public string DetailImage { get; set; }

        public System.Guid? ImageId { get; set; }

        //Price
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_PriceProductCode")]
        public string PriceProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Price")]
        public decimal? ProductPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductPostDate")]
        public DateTime? ProductPostDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductUserPost")]
        public string ProductUserPost { get; set; }

        //Warehouse
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public string WarehouseName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Color")]
        public string MainColorProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Color")]
        public string MainColorProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Style")]
        public string StyleWarehouseName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Quantity")]
        public decimal? Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductPostDate")]
        public DateTime? ProductWarehousePostDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductUserPost")]
        public string ProductWarehouseUserPost { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Inventory")]
        public Nullable<bool> isInventory { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_CompanyCode")]
        public string CompanyCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_Duration")]
        public Guid? WarrantyId { get; set; }

        public Guid? ProductWarrantyId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SerialNumber")]
        public string SerriNo { get; set; }
    }
}
