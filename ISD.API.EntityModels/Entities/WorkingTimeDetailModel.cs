using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkingTimeDetailModel", Schema = "ghService")]
    public partial class WorkingTimeDetailModel
    {
        [Key]
        public Guid WorkingTimeDetailId { get; set; }
        public Guid? WorkingTimeId { get; set; }
        public Guid? StoreId { get; set; }
        public TimeSpan? TimeFrameFrom { get; set; }
        public TimeSpan? TimeFrameTo { get; set; }
        public int? Amount { get; set; }

        [ForeignKey(nameof(WorkingTimeId))]
        [InverseProperty(nameof(WorkingTimeModel.WorkingTimeDetailModels))]
        public virtual WorkingTimeModel WorkingTime { get; set; }
    }
}
