using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StyleModel", Schema = "tSale")]
    public partial class StyleModel
    {
        public StyleModel()
        {
            ColorProductModels = new HashSet<ColorProductModel>();
            PriceProductModels = new HashSet<PriceProductModel>();
            WarehouseProductModels = new HashSet<WarehouseProductModel>();
        }

        [Key]
        public Guid StyleId { get; set; }
        [Required]
        [StringLength(50)]
        public string StyleCode { get; set; }
        [Required]
        [StringLength(100)]
        public string StyleName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }

        [InverseProperty(nameof(ColorProductModel.Style))]
        public virtual ICollection<ColorProductModel> ColorProductModels { get; set; }
        [InverseProperty(nameof(PriceProductModel.Style))]
        public virtual ICollection<PriceProductModel> PriceProductModels { get; set; }
        [InverseProperty(nameof(WarehouseProductModel.Style))]
        public virtual ICollection<WarehouseProductModel> WarehouseProductModels { get; set; }
    }
}
