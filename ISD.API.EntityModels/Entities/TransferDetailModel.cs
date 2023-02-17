using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TransferDetailModel", Schema = "Warehouse")]
    public partial class TransferDetailModel
    {
        [Key]
        public Guid TransferDetailId { get; set; }
        public Guid? TransferId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? FromStockId { get; set; }
        public Guid? ToStockId { get; set; }
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
        [InverseProperty(nameof(DimDateModel.TransferDetailModels))]
        public virtual DimDateModel DateKeyNavigation { get; set; }
        [ForeignKey(nameof(FromStockId))]
        [InverseProperty(nameof(StockModel.TransferDetailModelFromStocks))]
        public virtual StockModel FromStock { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.TransferDetailModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(ToStockId))]
        [InverseProperty(nameof(StockModel.TransferDetailModelToStocks))]
        public virtual StockModel ToStock { get; set; }
    }
}
