using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RemindTaskModel", Schema = "Task")]
    public partial class RemindTaskModel
    {
        [Key]
        public Guid TaskId { get; set; }
        public Guid? ParentTaskId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RemindDate { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
