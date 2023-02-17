using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PromotionProductModel", Schema = "tSale")]
    public partial class PromotionProductModel
    {
        [Key]
        public Guid PromotionId { get; set; }
        [Key]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.PromotionProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(PromotionId))]
        [InverseProperty(nameof(PromotionModel.PromotionProductModels))]
        public virtual PromotionModel Promotion { get; set; }
    }
}
