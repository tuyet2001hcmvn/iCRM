using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessoryProductModel", Schema = "tSale")]
    public partial class AccessoryProductModel
    {
        [Key]
        public Guid AccessoryProductId { get; set; }
        public Guid ProductId { get; set; }
        public Guid AccessoryId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        [ForeignKey(nameof(AccessoryId))]
        [InverseProperty(nameof(AccessoryModel.AccessoryProductModels))]
        public virtual AccessoryModel Accessory { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.AccessoryProductModels))]
        public virtual ProductModel Product { get; set; }
    }
}
