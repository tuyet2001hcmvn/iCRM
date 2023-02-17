using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StoreModel", Schema = "tMasterData")]
    public partial class StoreModel
    {
        public StoreModel()
        {
            AccountInStoreModels = new HashSet<AccountInStoreModel>();
            DeliveryModels = new HashSet<DeliveryModel>();
            DepartmentModels = new HashSet<DepartmentModel>();
            InventoryModels = new HashSet<InventoryModel>();
            MaterialMinMaxPriceBySaleOrgModels = new HashSet<MaterialMinMaxPriceBySaleOrgModel>();
            PromotionByStoreModels = new HashSet<PromotionByStoreModel>();
            SalesEmployeeModels = new HashSet<SalesEmployeeModel>();
            StockReceivingMasterModels = new HashSet<StockReceivingMasterModel>();
            StockStoreMappings = new HashSet<StockStoreMapping>();
            StockTransferRequestModels = new HashSet<StockTransferRequestModel>();
            TransferModels = new HashSet<TransferModel>();
        }

        [Key]
        public Guid StoreId { get; set; }
        [StringLength(50)]
        public string SaleOrgCode { get; set; }
        public Guid? StoreTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        public Guid CompanyId { get; set; }
        [StringLength(100)]
        public string TelProduct { get; set; }
        [StringLength(100)]
        public string TelService { get; set; }
        [StringLength(500)]
        public string StoreAddress { get; set; }
        [StringLength(50)]
        public string Area { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [StringLength(1000)]
        public string LogoUrl { get; set; }
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        [Column("mLat")]
        [StringLength(50)]
        public string MLat { get; set; }
        [Column("mLong")]
        [StringLength(50)]
        public string MLong { get; set; }
        [StringLength(200)]
        public string InvoiceStoreName { get; set; }
        [StringLength(50)]
        public string DefaultCustomerSource { get; set; }
        [Column("SMSTemplateCode")]
        [StringLength(50)]
        public string SmstemplateCode { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.StoreModels))]
        public virtual CompanyModel Company { get; set; }
        [InverseProperty(nameof(AccountInStoreModel.Store))]
        public virtual ICollection<AccountInStoreModel> AccountInStoreModels { get; set; }
        [InverseProperty(nameof(DeliveryModel.Store))]
        public virtual ICollection<DeliveryModel> DeliveryModels { get; set; }
        [InverseProperty(nameof(DepartmentModel.Store))]
        public virtual ICollection<DepartmentModel> DepartmentModels { get; set; }
        [InverseProperty(nameof(InventoryModel.Store))]
        public virtual ICollection<InventoryModel> InventoryModels { get; set; }
        [InverseProperty(nameof(MaterialMinMaxPriceBySaleOrgModel.Store))]
        public virtual ICollection<MaterialMinMaxPriceBySaleOrgModel> MaterialMinMaxPriceBySaleOrgModels { get; set; }
        [InverseProperty(nameof(PromotionByStoreModel.Store))]
        public virtual ICollection<PromotionByStoreModel> PromotionByStoreModels { get; set; }
        [InverseProperty(nameof(SalesEmployeeModel.Store))]
        public virtual ICollection<SalesEmployeeModel> SalesEmployeeModels { get; set; }
        [InverseProperty(nameof(StockReceivingMasterModel.Store))]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModels { get; set; }
        [InverseProperty(nameof(StockStoreMapping.Store))]
        public virtual ICollection<StockStoreMapping> StockStoreMappings { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.Store))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModels { get; set; }
        [InverseProperty(nameof(TransferModel.Store))]
        public virtual ICollection<TransferModel> TransferModels { get; set; }
    }
}
