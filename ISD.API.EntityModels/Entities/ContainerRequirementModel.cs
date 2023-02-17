using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ContainerRequirementModel", Schema = "ghMasterData")]
    public partial class ContainerRequirementModel
    {
        public ContainerRequirementModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string ContainerRequirementCode { get; set; }
        [StringLength(400)]
        public string ContainerRequirementName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.ContainerRequirementCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
