using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkingTimeModel", Schema = "ghService")]
    public partial class WorkingTimeModel
    {
        public WorkingTimeModel()
        {
            WorkingTimeDetailModels = new HashSet<WorkingTimeDetailModel>();
        }

        [Key]
        public Guid WorkingTimeId { get; set; }
        public int? DayOfWeek { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }

        [InverseProperty(nameof(WorkingTimeDetailModel.WorkingTime))]
        public virtual ICollection<WorkingTimeDetailModel> WorkingTimeDetailModels { get; set; }
    }
}
