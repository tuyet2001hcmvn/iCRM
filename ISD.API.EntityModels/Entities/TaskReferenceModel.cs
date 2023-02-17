using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskReferenceModel", Schema = "Task")]
    public partial class TaskReferenceModel
    {
        [Key]
        public Guid TaskReferenceId { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        public Guid? ObjectId { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [StringLength(50)]
        public string RolesCode { get; set; }
    }
}
