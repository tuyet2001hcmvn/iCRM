using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerPromotionProductModel", Schema = "tSale")]
    public partial class CustomerPromotionProductModel
    {
        [Key]
        public Guid PromotionId { get; set; }
        [Key]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.CustomerPromotionProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(PromotionId))]
        [InverseProperty(nameof(CustomerPromotionModel.CustomerPromotionProductModels))]
        public virtual CustomerPromotionModel Promotion { get; set; }
    }
}
