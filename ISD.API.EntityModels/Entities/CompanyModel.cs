using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CompanyModel", Schema = "tMasterData")]
    public partial class CompanyModel
    {
        public CompanyModel()
        {
            DeliveryModels = new HashSet<DeliveryModel>();
            InventoryModels = new HashSet<InventoryModel>();
            ProductModels = new HashSet<ProductModel>();
            SalesEmployeeModels = new HashSet<SalesEmployeeModel>();
            StockReceivingMasterModels = new HashSet<StockReceivingMasterModel>();
            StockTransferRequestModels = new HashSet<StockTransferRequestModel>();
            StoreModels = new HashSet<StoreModel>();
            TransferModels = new HashSet<TransferModel>();
        }

        [Key]
        public Guid CompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [StringLength(50)]
        public string Plant { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(100)]
        public string TelProduct { get; set; }
        [StringLength(100)]
        public string TelService { get; set; }
        [StringLength(500)]
        public string CompanyAddress { get; set; }
        [StringLength(100)]
        public string Logo { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(50)]
        public string TaxCode { get; set; }
        [StringLength(100)]
        public string CompanyShortName { get; set; }
        [Column("SMSTemplateCode")]
        [StringLength(50)]
        public string SmstemplateCode { get; set; }

        [InverseProperty(nameof(DeliveryModel.Company))]
        public virtual ICollection<DeliveryModel> DeliveryModels { get; set; }
        [InverseProperty(nameof(InventoryModel.Company))]
        public virtual ICollection<InventoryModel> InventoryModels { get; set; }
        [InverseProperty(nameof(ProductModel.Company))]
        public virtual ICollection<ProductModel> ProductModels { get; set; }
        [InverseProperty(nameof(SalesEmployeeModel.Company))]
        public virtual ICollection<SalesEmployeeModel> SalesEmployeeModels { get; set; }
        [InverseProperty(nameof(StockReceivingMasterModel.Company))]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModels { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.Company))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModels { get; set; }
        [InverseProperty(nameof(StoreModel.Company))]
        public virtual ICollection<StoreModel> StoreModels { get; set; }
        [InverseProperty(nameof(TransferModel.Company))]
        public virtual ICollection<TransferModel> TransferModels { get; set; }
    }
}
