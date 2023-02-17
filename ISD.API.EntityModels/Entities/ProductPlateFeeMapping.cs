using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Product_PlateFee_Mapping", Schema = "tSale")]
    public partial class ProductPlateFeeMapping
    {
        [Key]
        public Guid PlateFeeId { get; set; }
        [Key]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(PlateFeeId))]
        [InverseProperty(nameof(PlateFeeModel.ProductPlateFeeMappings))]
        public virtual PlateFeeModel PlateFee { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.ProductPlateFeeMappings))]
        public virtual ProductModel Product { get; set; }
    }
}
