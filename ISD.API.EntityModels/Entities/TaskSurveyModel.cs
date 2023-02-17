using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskSurveyModel", Schema = "Task")]
    public partial class TaskSurveyModel
    {
        [Key]
        public Guid TaskSurveyId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? SurveyId { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
