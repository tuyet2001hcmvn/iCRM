using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WarehouseProductModel", Schema = "tSale")]
    public partial class WarehouseProductModel
    {
        [Key]
        public Guid WarehouseProductId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? MainColorId { get; set; }
        public Guid? StyleId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PostDate { get; set; }
        public TimeSpan? PostTime { get; set; }
        [StringLength(50)]
        public string UserPost { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.WarehouseProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty(nameof(StyleModel.WarehouseProductModels))]
        public virtual StyleModel Style { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        [InverseProperty(nameof(WarehouseModel.WarehouseProductModels))]
        public virtual WarehouseModel Warehouse { get; set; }
    }
}
