using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskReporterModel", Schema = "Task")]
    public partial class TaskReporterModel
    {
        [Key]
        public Guid TaskReporterId { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        [StringLength(10)]
        public string TaskAssignTypeCode { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [StringLength(50)]
        public string RolesCode { get; set; }
    }
}
