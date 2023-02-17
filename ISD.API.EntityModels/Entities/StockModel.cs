using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StockModel", Schema = "Warehouse")]
    public partial class StockModel
    {
        public StockModel()
        {
            DeliveryDetailModels = new HashSet<DeliveryDetailModel>();
            StockReceivingDetailModels = new HashSet<StockReceivingDetailModel>();
            StockStoreMappings = new HashSet<StockStoreMapping>();
            StockTransferRequestModelFromStockNavigations = new HashSet<StockTransferRequestModel>();
            StockTransferRequestModelToStockNavigations = new HashSet<StockTransferRequestModel>();
            TransferDetailModelFromStocks = new HashSet<TransferDetailModel>();
            TransferDetailModelToStocks = new HashSet<TransferDetailModel>();
        }

        [Key]
        public Guid StockId { get; set; }
        [StringLength(50)]
        public string StockCode { get; set; }
        [StringLength(100)]
        public string StockName { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(DeliveryDetailModel.Stock))]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModels { get; set; }
        [InverseProperty(nameof(StockReceivingDetailModel.Stock))]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
        [InverseProperty(nameof(StockStoreMapping.Stock))]
        public virtual ICollection<StockStoreMapping> StockStoreMappings { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.FromStockNavigation))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModelFromStockNavigations { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.ToStockNavigation))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModelToStockNavigations { get; set; }
        [InverseProperty(nameof(TransferDetailModel.FromStock))]
        public virtual ICollection<TransferDetailModel> TransferDetailModelFromStocks { get; set; }
        [InverseProperty(nameof(TransferDetailModel.ToStock))]
        public virtual ICollection<TransferDetailModel> TransferDetailModelToStocks { get; set; }
    }
}
