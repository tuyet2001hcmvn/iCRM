//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductModel()
        {
            this.ProductWarrantyModel = new HashSet<ProductWarrantyModel>();
            this.TemplateAndGiftMemberAddressModel = new HashSet<TemplateAndGiftMemberAddressModel>();
            this.AccessoryProductModel = new HashSet<AccessoryProductModel>();
            this.ColorProductModel = new HashSet<ColorProductModel>();
            this.ImageProductModel = new HashSet<ImageProductModel>();
            this.PriceProductModel = new HashSet<PriceProductModel>();
            this.DeliveryDetailModel = new HashSet<DeliveryDetailModel>();
            this.PropertiesProductModel = new HashSet<PropertiesProductModel>();
            this.SpecificationsProductModel = new HashSet<SpecificationsProductModel>();
            this.StockReceivingDetailModel = new HashSet<StockReceivingDetailModel>();
            this.StockTransferRequestDetailModel = new HashSet<StockTransferRequestDetailModel>();
            this.TransferDetailModel = new HashSet<TransferDetailModel>();
            this.WarehouseProductModel = new HashSet<WarehouseProductModel>();
            this.CustomerPromotionModel = new HashSet<CustomerPromotionModel>();
            this.PeriodicallyCheckingModel = new HashSet<PeriodicallyCheckingModel>();
            this.PlateFeeModel = new HashSet<PlateFeeModel>();
            this.PromotionModel = new HashSet<PromotionModel>();
        }
    
        public System.Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ERPProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.Guid> BrandId { get; set; }
        public Nullable<decimal> CylinderCapacity { get; set; }
        public Nullable<System.Guid> CategoryId { get; set; }
        public System.Guid ConfigurationId { get; set; }
        public string GuaranteePeriod { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<bool> isHot { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
        public Nullable<System.Guid> ParentCategoryId { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }
        public Nullable<bool> isInventory { get; set; }
        public Nullable<decimal> SampleValue { get; set; }
        public Nullable<decimal> ProcessingValue { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.Guid> WarrantyId { get; set; }
        public string ProductGroups { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductWarrantyModel> ProductWarrantyModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModel { get; set; }
        public virtual CompanyModel CompanyModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessoryProductModel> AccessoryProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ColorProductModel> ColorProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImageProductModel> ImageProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceProductModel> PriceProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertiesProductModel> PropertiesProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecificationsProductModel> SpecificationsProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferRequestDetailModel> StockTransferRequestDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferDetailModel> TransferDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseProductModel> WarehouseProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPromotionModel> CustomerPromotionModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodicallyCheckingModel> PeriodicallyCheckingModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlateFeeModel> PlateFeeModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionModel> PromotionModel { get; set; }
    }
}