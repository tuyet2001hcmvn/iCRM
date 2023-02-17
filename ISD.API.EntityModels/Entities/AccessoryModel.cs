using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessoryModel", Schema = "tSale")]
    public partial class AccessoryModel
    {
        public AccessoryModel()
        {
            AccessoryDetailModels = new HashSet<AccessoryDetailModel>();
            AccessoryProductModels = new HashSet<AccessoryProductModel>();
        }

        [Key]
        public Guid AccessoryId { get; set; }
        public Guid AccessoryCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string AccessoryCode { get; set; }
        [Required]
        [StringLength(100)]
        public string AccessoryName { get; set; }
        [Column("isHelmet")]
        public bool? IsHelmet { get; set; }
        [Column("isHelmetAdult")]
        public bool? IsHelmetAdult { get; set; }
        [StringLength(50)]
        public string Size { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }

        [InverseProperty(nameof(AccessoryDetailModel.Accessory))]
        public virtual ICollection<AccessoryDetailModel> AccessoryDetailModels { get; set; }
        [InverseProperty(nameof(AccessoryProductModel.Accessory))]
        public virtual ICollection<AccessoryProductModel> AccessoryProductModels { get; set; }
    }
}
