using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductModel", Schema = "tSale")]
    public partial class ProductModel
    {
        public ProductModel()
        {
            AccessoryProductModels = new HashSet<AccessoryProductModel>();
            ColorProductModels = new HashSet<ColorProductModel>();
            CustomerPromotionProductModels = new HashSet<CustomerPromotionProductModel>();
            DeliveryDetailModels = new HashSet<DeliveryDetailModel>();
            ImageProductModels = new HashSet<ImageProductModel>();
            PriceProductModels = new HashSet<PriceProductModel>();
            ProductPeriodicallyCheckingMappings = new HashSet<ProductPeriodicallyCheckingMapping>();
            ProductPlateFeeMappings = new HashSet<ProductPlateFeeMapping>();
            ProductWarrantyModels = new HashSet<ProductWarrantyModel>();
            PromotionProductModels = new HashSet<PromotionProductModel>();
            PropertiesProductModels = new HashSet<PropertiesProductModel>();
            SpecificationsProductModels = new HashSet<SpecificationsProductModel>();
            StockReceivingDetailModels = new HashSet<StockReceivingDetailModel>();
            StockTransferRequestDetailModels = new HashSet<StockTransferRequestDetailModel>();
            TemplateAndGiftMemberAddressModels = new HashSet<TemplateAndGiftMemberAddressModel>();
            TransferDetailModels = new HashSet<TransferDetailModel>();
            WarehouseProductModels = new HashSet<WarehouseProductModel>();
        }

        [Key]
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }
        [Column("ERPProductCode")]
        [StringLength(50)]
        public string ErpproductCode { get; set; }
        [Required]
        [StringLength(4000)]
        public string ProductName { get; set; }
        public Guid? BrandId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CylinderCapacity { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid ConfigurationId { get; set; }
        [StringLength(50)]
        public string GuaranteePeriod { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [Column("isHot")]
        public bool? IsHot { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public Guid? CompanyId { get; set; }
        [Column("isInventory")]
        public bool? IsInventory { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SampleValue { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ProcessingValue { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public Guid? WarrantyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.ProductModels))]
        public virtual CompanyModel Company { get; set; }
        [InverseProperty(nameof(AccessoryProductModel.Product))]
        public virtual ICollection<AccessoryProductModel> AccessoryProductModels { get; set; }
        [InverseProperty(nameof(ColorProductModel.Product))]
        public virtual ICollection<ColorProductModel> ColorProductModels { get; set; }
        [InverseProperty(nameof(CustomerPromotionProductModel.Product))]
        public virtual ICollection<CustomerPromotionProductModel> CustomerPromotionProductModels { get; set; }
        [InverseProperty(nameof(DeliveryDetailModel.Product))]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModels { get; set; }
        [InverseProperty(nameof(ImageProductModel.Product))]
        public virtual ICollection<ImageProductModel> ImageProductModels { get; set; }
        [InverseProperty(nameof(PriceProductModel.Product))]
        public virtual ICollection<PriceProductModel> PriceProductModels { get; set; }
        [InverseProperty(nameof(ProductPeriodicallyCheckingMapping.Product))]
        public virtual ICollection<ProductPeriodicallyCheckingMapping> ProductPeriodicallyCheckingMappings { get; set; }
        [InverseProperty(nameof(ProductPlateFeeMapping.Product))]
        public virtual ICollection<ProductPlateFeeMapping> ProductPlateFeeMappings { get; set; }
        [InverseProperty(nameof(ProductWarrantyModel.Product))]
        public virtual ICollection<ProductWarrantyModel> ProductWarrantyModels { get; set; }
        [InverseProperty(nameof(PromotionProductModel.Product))]
        public virtual ICollection<PromotionProductModel> PromotionProductModels { get; set; }
        [InverseProperty(nameof(PropertiesProductModel.Product))]
        public virtual ICollection<PropertiesProductModel> PropertiesProductModels { get; set; }
        [InverseProperty(nameof(SpecificationsProductModel.Product))]
        public virtual ICollection<SpecificationsProductModel> SpecificationsProductModels { get; set; }
        [InverseProperty(nameof(StockReceivingDetailModel.Product))]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
        [InverseProperty(nameof(StockTransferRequestDetailModel.Product))]
        public virtual ICollection<StockTransferRequestDetailModel> StockTransferRequestDetailModels { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberAddressModel.Product))]
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModels { get; set; }
        [InverseProperty(nameof(TransferDetailModel.Product))]
        public virtual ICollection<TransferDetailModel> TransferDetailModels { get; set; }
        [InverseProperty(nameof(WarehouseProductModel.Product))]
        public virtual ICollection<WarehouseProductModel> WarehouseProductModels { get; set; }
    }
}
