using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Survey_Mapping", Schema = "Task")]
    public partial class SurveyMapping
    {
        [Key]
        public Guid SurveyMappingId { get; set; }
        public Guid? SurveyId { get; set; }
        public Guid? CustomReferences { get; set; }
        public int? OrderIndex { get; set; }
    }
}
