using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ImageProductModel", Schema = "tSale")]
    public partial class ImageProductModel
    {
        [Key]
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ColorProductId { get; set; }
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        [Column("isDefault")]
        public bool? IsDefault { get; set; }

        [ForeignKey(nameof(ColorProductId))]
        [InverseProperty(nameof(ColorProductModel.ImageProductModels))]
        public virtual ColorProductModel ColorProduct { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.ImageProductModels))]
        public virtual ProductModel Product { get; set; }
    }
}
