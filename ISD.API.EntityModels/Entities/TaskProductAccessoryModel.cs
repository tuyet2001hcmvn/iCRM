using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskProductAccessoryModel", Schema = "Task")]
    public partial class TaskProductAccessoryModel
    {
        [Key]
        public Guid TaskProductAccessoryId { get; set; }
        public Guid? TaskProductId { get; set; }
        public Guid? AccessoryId { get; set; }
        public int? Qty { get; set; }
        [StringLength(50)]
        public string ProductAccessoryTypeCode { get; set; }
        [StringLength(50)]
        public string AccErrorTypeCode { get; set; }

        [ForeignKey(nameof(TaskProductId))]
        [InverseProperty(nameof(TaskProductModel.TaskProductAccessoryModels))]
        public virtual TaskProductModel TaskProduct { get; set; }
    }
}
