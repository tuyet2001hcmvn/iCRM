using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SpecificationsModel", Schema = "tSale")]
    public partial class SpecificationsModel
    {
        public SpecificationsModel()
        {
            SpecificationsProductModels = new HashSet<SpecificationsProductModel>();
        }

        [Key]
        public Guid SpecificationsId { get; set; }
        [Required]
        [StringLength(50)]
        public string SpecificationsCode { get; set; }
        [Required]
        [StringLength(100)]
        public string SpecificationsName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }

        [InverseProperty(nameof(SpecificationsProductModel.Specifications))]
        public virtual ICollection<SpecificationsProductModel> SpecificationsProductModels { get; set; }
    }
}
