using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewTaskGtb
    {
        public Guid TaskId { get; set; }
        public int TaskCode { get; set; }
        [Required]
        [StringLength(4000)]
        public string Summary { get; set; }
        public Guid? ChildTaskId { get; set; }
        public int? ChildTaskCode { get; set; }
        [StringLength(4000)]
        public string ChildTaskSummary { get; set; }
        [StringLength(4000)]
        public string ChildTaskDescription { get; set; }
        public string AssigneeName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RemindDate { get; set; }
    }
}
