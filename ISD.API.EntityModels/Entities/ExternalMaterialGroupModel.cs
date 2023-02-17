using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ExternalMaterialGroupModel", Schema = "ghMasterData")]
    public partial class ExternalMaterialGroupModel
    {
        public ExternalMaterialGroupModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string ExternalMaterialGroupCode { get; set; }
        [StringLength(400)]
        public string ExternalMaterialGroupName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.ExternalMaterialGroupCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
