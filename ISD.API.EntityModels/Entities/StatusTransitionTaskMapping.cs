using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StatusTransition_Task_Mapping", Schema = "Task")]
    public partial class StatusTransitionTaskMapping
    {
        [Key]
        public Guid TaskTransitionLogId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? FromStatusId { get; set; }
        public Guid? ToStatusId { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number1 { get; set; }
        public Guid? ApproveBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApproveTime { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
    }
}
