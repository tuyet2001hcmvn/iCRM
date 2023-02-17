using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CheckInOutModel", Schema = "Task")]
    public partial class CheckInOutModel
    {
        [Key]
        public Guid CheckInOutId { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(200)]
        public string CheckInLat { get; set; }
        [StringLength(200)]
        public string CheckInLng { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CheckInDistance { get; set; }
        public Guid? CheckInBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckInTime { get; set; }
        [StringLength(200)]
        public string CheckOutLat { get; set; }
        [StringLength(200)]
        public string CheckOutLng { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CheckOutDistance { get; set; }
        public Guid? CheckOutBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckOutTime { get; set; }
    }
}
