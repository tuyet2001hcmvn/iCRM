using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialAccessoryMappingModel", Schema = "ghMasterData")]
    public partial class MaterialAccessoryMappingModel
    {
        [Key]
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Key]
        public Guid AccessoryId { get; set; }

        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.MaterialAccessoryMappingModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
    }
}
