using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CareerModel", Schema = "ghMasterData")]
    public partial class CareerModel
    {
        [Key]
        public Guid CareerId { get; set; }
        [StringLength(50)]
        public string CareerCode { get; set; }
        [StringLength(1000)]
        public string CareerName { get; set; }
        public bool? Actived { get; set; }
    }
}
