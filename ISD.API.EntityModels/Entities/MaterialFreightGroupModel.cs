using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialFreightGroupModel", Schema = "ghMasterData")]
    public partial class MaterialFreightGroupModel
    {
        public MaterialFreightGroupModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string MaterialFreightGroupCode { get; set; }
        [StringLength(500)]
        public string MaterialFreightGroupName { get; set; }
        [Column("RGBCode")]
        [StringLength(50)]
        public string Rgbcode { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.MaterialFreightGroupCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
