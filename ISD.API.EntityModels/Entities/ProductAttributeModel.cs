using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductAttributeModel", Schema = "tSale")]
    public partial class ProductAttributeModel
    {
        [Key]
        public Guid ProductId { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(100)]
        public string Color { get; set; }
        [StringLength(100)]
        public string Thickness { get; set; }
        [StringLength(100)]
        public string Allocation { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(100)]
        public string Surface { get; set; }
        [StringLength(100)]
        public string NumberOfSurface { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GrossWeight { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? NetWeight { get; set; }
        [StringLength(100)]
        public string WeightUnit { get; set; }
    }
}
