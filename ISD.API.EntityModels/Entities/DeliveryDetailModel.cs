using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("DeliveryDetailModel", Schema = "Warehouse")]
    public partial class DeliveryDetailModel
    {
        [Key]
        public Guid DeliveryDetailId { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? StockId { get; set; }
        public Guid? InventoryId { get; set; }
        public int? DateKey { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? UnitPrice { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(DateKey))]
        [InverseProperty(nameof(DimDateModel.DeliveryDetailModels))]
        public virtual DimDateModel DateKeyNavigation { get; set; }
        [ForeignKey(nameof(DeliveryId))]
        [InverseProperty(nameof(DeliveryModel.DeliveryDetailModels))]
        public virtual DeliveryModel Delivery { get; set; }
        [ForeignKey(nameof(InventoryId))]
        [InverseProperty(nameof(InventoryModel.DeliveryDetailModels))]
        public virtual InventoryModel Inventory { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.DeliveryDetailModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(StockId))]
        [InverseProperty(nameof(StockModel.DeliveryDetailModels))]
        public virtual StockModel Stock { get; set; }
    }
}
