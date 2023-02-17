using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TemperatureConditionModel", Schema = "ghMasterData")]
    public partial class TemperatureConditionModel
    {
        public TemperatureConditionModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string TemperatureConditionCode { get; set; }
        [StringLength(400)]
        public string TemperatureConditionName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.TemperatureConditionCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
