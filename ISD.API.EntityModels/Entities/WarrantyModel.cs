using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WarrantyModel", Schema = "Maintenance")]
    public partial class WarrantyModel
    {
        public WarrantyModel()
        {
            ProductWarrantyModels = new HashSet<ProductWarrantyModel>();
        }

        [Key]
        public Guid WarrantyId { get; set; }
        [Required]
        [StringLength(50)]
        public string WarrantyCode { get; set; }
        [Required]
        [StringLength(200)]
        public string WarrantyName { get; set; }
        [StringLength(200)]
        public string Coverage { get; set; }
        public int Duration { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(ProductWarrantyModel.Warranty))]
        public virtual ICollection<ProductWarrantyModel> ProductWarrantyModels { get; set; }
    }
}
