using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfitCenterModel", Schema = "ghMasterData")]
    public partial class ProfitCenterModel
    {
        public ProfitCenterModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string ProfitCenterCode { get; set; }
        [StringLength(400)]
        public string ProfitCenterName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.ProfitCenterCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
