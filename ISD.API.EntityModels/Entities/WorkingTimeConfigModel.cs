using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkingTimeConfigModel", Schema = "ghService")]
    public partial class WorkingTimeConfigModel
    {
        [Key]
        public Guid WorkingTimeConfigId { get; set; }
        public Guid? StoreId { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public int? Interval { get; set; }
    }
}
