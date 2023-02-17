using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PrognosisModel", Schema = "tMasterData")]
    public partial class PrognosisModel
    {
        [Key]
        public Guid PrognosisId { get; set; }
        [Required]
        [StringLength(100)]
        public string PrognosisDescription { get; set; }
    }
}
