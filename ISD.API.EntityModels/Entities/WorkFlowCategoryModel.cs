using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkFlowCategoryModel", Schema = "Task")]
    public partial class WorkFlowCategoryModel
    {
        [Key]
        [StringLength(50)]
        public string WorkFlowCategoryCode { get; set; }
        [StringLength(500)]
        public string WorkFlowCategoryName { get; set; }
        public string ReportType { get; set; }
    }
}
