using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PriceProductModel", Schema = "tSale")]
    public partial class PriceProductModel
    {
        [Key]
        public Guid PriceProductId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? StyleId { get; set; }
        public Guid? MainColorId { get; set; }
        [StringLength(100)]
        public string PriceProductCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PostDate { get; set; }
        public TimeSpan? PostTime { get; set; }
        [StringLength(50)]
        public string UserPost { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.PriceProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty(nameof(StyleModel.PriceProductModels))]
        public virtual StyleModel Style { get; set; }
    }
}
