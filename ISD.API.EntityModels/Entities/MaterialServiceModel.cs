using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialServiceModel", Schema = "ghMasterData")]
    public partial class MaterialServiceModel
    {
        public MaterialServiceModel()
        {
            MaterialServiceMappingModels = new HashSet<MaterialServiceMappingModel>();
        }

        [Key]
        [StringLength(100)]
        public string MaterialNumber { get; set; }
        [StringLength(200)]
        public string ShortText { get; set; }
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [StringLength(10)]
        public string MaterialType { get; set; }

        [InverseProperty(nameof(MaterialServiceMappingModel.MaterialNumberNavigation))]
        public virtual ICollection<MaterialServiceMappingModel> MaterialServiceMappingModels { get; set; }
    }
}
