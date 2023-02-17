using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialGroupModel", Schema = "ghMasterData")]
    public partial class MaterialGroupModel
    {
        public MaterialGroupModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string MaterialGroupCode { get; set; }
        [StringLength(400)]
        public string MaterialGroupName { get; set; }
        [StringLength(100)]
        public string IconUrl { get; set; }
        public bool? Actived { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ServiceFee { get; set; }

        [InverseProperty(nameof(MaterialModel.MaterialGroupCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
