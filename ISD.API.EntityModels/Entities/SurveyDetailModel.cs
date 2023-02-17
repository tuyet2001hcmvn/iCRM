using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SurveyDetailModel", Schema = "tMasterData")]
    public partial class SurveyDetailModel
    {
        [Key]
        public Guid SurveyDetailId { get; set; }
        public Guid? SurveyId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerValue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AnswerDatetime { get; set; }
        public int? OrderIndex { get; set; }
    }
}
