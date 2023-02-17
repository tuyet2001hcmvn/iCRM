using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialSpecificationsModel", Schema = "ghMasterData")]
    public partial class MaterialSpecificationsModel
    {
        [Key]
        public Guid MaterialSpecificationsId { get; set; }
        public Guid? SpecificationsId { get; set; }
        [StringLength(40)]
        public string MaterialCode { get; set; }
        public string Description { get; set; }
    }
}
