using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SurveyModel", Schema = "tMasterData")]
    public partial class SurveyModel
    {
        [Key]
        public Guid SurveyId { get; set; }
        [StringLength(500)]
        public string Question { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
    }
}
