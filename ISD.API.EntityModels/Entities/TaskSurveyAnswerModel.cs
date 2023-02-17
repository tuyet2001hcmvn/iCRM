using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskSurveyAnswerModel", Schema = "Task")]
    public partial class TaskSurveyAnswerModel
    {
        [Key]
        public Guid TaskSurveyAnswerId { get; set; }
        public Guid TaskSurveyId { get; set; }
        public Guid? SurveyDetailId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerValue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AnswerDatetime { get; set; }
    }
}
