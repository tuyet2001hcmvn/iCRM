using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StockTransferRequestDetailModel", Schema = "Warehouse")]
    public partial class StockTransferRequestDetailModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? StockTransferRequestId { get; set; }
        public Guid? ProductId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RequestQuantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? OfferQuantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TransferredQuantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? UnitPrice { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [StringLength(500)]
        public string Note { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.StockTransferRequestDetailModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(StockTransferRequestId))]
        [InverseProperty(nameof(StockTransferRequestModel.StockTransferRequestDetailModels))]
        public virtual StockTransferRequestModel StockTransferRequest { get; set; }
    }
}
