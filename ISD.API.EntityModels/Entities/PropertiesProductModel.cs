using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PropertiesProductModel", Schema = "tSale")]
    public partial class PropertiesProductModel
    {
        [Key]
        public Guid PropertiesId { get; set; }
        public Guid ProductId { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(50)]
        public string SubjectColor { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(50)]
        public string DescriptionColor { get; set; }
        [StringLength(100)]
        public string Image { get; set; }
        [StringLength(50)]
        public string BackgroundColor { get; set; }
        [StringLength(10)]
        public string X { get; set; }
        [StringLength(10)]
        public string Y { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.PropertiesProductModels))]
        public virtual ProductModel Product { get; set; }
    }
}
