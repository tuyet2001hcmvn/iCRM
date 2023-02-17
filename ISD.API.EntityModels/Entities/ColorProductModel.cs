using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ColorProductModel", Schema = "tSale")]
    public partial class ColorProductModel
    {
        public ColorProductModel()
        {
            ImageProductModels = new HashSet<ImageProductModel>();
        }

        [Key]
        public Guid ColorProductId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? StyleId { get; set; }
        public Guid? MainColorId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.ColorProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty(nameof(StyleModel.ColorProductModels))]
        public virtual StyleModel Style { get; set; }
        [InverseProperty(nameof(ImageProductModel.ColorProduct))]
        public virtual ICollection<ImageProductModel> ImageProductModels { get; set; }
    }
}
