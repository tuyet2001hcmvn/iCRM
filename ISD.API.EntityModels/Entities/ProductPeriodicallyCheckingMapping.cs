using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Product_PeriodicallyChecking_Mapping", Schema = "tSale")]
    public partial class ProductPeriodicallyCheckingMapping
    {
        [Key]
        public Guid PeriodicallyCheckingId { get; set; }
        [Key]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(PeriodicallyCheckingId))]
        [InverseProperty(nameof(PeriodicallyCheckingModel.ProductPeriodicallyCheckingMappings))]
        public virtual PeriodicallyCheckingModel PeriodicallyChecking { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.ProductPeriodicallyCheckingMappings))]
        public virtual ProductModel Product { get; set; }
    }
}
