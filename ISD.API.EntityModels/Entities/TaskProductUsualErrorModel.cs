using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskProductUsualErrorModel", Schema = "Task")]
    public partial class TaskProductUsualErrorModel
    {
        [Key]
        public Guid TaskProductUsualErrorId { get; set; }
        public Guid? TaskProductId { get; set; }
        [StringLength(50)]
        public string UsualErrorCode { get; set; }

        [ForeignKey(nameof(TaskProductId))]
        [InverseProperty(nameof(TaskProductModel.TaskProductUsualErrorModels))]
        public virtual TaskProductModel TaskProduct { get; set; }
    }
}
