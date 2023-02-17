using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("LaborModel", Schema = "ghMasterData")]
    public partial class LaborModel
    {
        public LaborModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string LaborCode { get; set; }
        [StringLength(400)]
        public string LaborName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.LaborCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
