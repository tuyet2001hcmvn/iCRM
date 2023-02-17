using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("Task_GTB_TempModel", Schema = "Task")]
    public partial class TaskGtbTempModel
    {
        [StringLength(50)]
        public string TaskCode { get; set; }
        [StringLength(4000)]
        public string Summary { get; set; }
        public Guid? WorkFlowId { get; set; }
        public Guid? TaskStatusId { get; set; }
        [StringLength(50)]
        public string ProfileForeignCode { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(4000)]
        public string VisitAddress { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReceiveDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [StringLength(50)]
        public string Reporter { get; set; }
        [StringLength(50)]
        public string Assignee { get; set; }
    }
}
