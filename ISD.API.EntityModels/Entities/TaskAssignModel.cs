using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskAssignModel", Schema = "Task")]
    public partial class TaskAssignModel
    {
        [Key]
        public Guid TaskAssignId { get; set; }
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
        public Guid? GroupId { get; set; }
    }
}
