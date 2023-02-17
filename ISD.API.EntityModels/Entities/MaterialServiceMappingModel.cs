using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialServiceMappingModel", Schema = "ghMasterData")]
    public partial class MaterialServiceMappingModel
    {
        [Key]
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Key]
        [StringLength(100)]
        public string MaterialNumber { get; set; }

        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.MaterialServiceMappingModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
        [ForeignKey(nameof(MaterialNumber))]
        [InverseProperty(nameof(MaterialServiceModel.MaterialServiceMappingModels))]
        public virtual MaterialServiceModel MaterialNumberNavigation { get; set; }
    }
}
