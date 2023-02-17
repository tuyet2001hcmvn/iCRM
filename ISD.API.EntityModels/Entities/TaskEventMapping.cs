using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Task_Event_Mapping", Schema = "Task")]
    public partial class TaskEventMapping
    {
        [Key]
        public Guid TaskEventMappingId { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(100)]
        public string EventId { get; set; }
        public Guid? AccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
