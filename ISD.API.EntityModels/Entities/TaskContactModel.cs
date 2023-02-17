using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskContactModel", Schema = "Task")]
    public partial class TaskContactModel
    {
        [Key]
        public Guid TaskContactId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? ContactId { get; set; }
        [Column("isMain")]
        public bool? IsMain { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
    }
}
