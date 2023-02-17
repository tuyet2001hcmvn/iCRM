using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("FaceCheckInOutModel")]
    public partial class FaceCheckInOutModel
    {
        [Key]
        [StringLength(500)]
        public string FaceId { get; set; }
        [StringLength(250)]
        public string PersonName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public TimeSpan? CheckinTime { get; set; }
        [Column("AliasID")]
        [StringLength(50)]
        public string AliasId { get; set; }
        [Column("PlaceID")]
        [StringLength(50)]
        public string PlaceId { get; set; }
        [Column("PersonID")]
        [StringLength(150)]
        public string PersonId { get; set; }
        [Column("avatar")]
        [StringLength(500)]
        public string Avatar { get; set; }
        [StringLength(250)]
        public string Place { get; set; }
        [Column("type")]
        public int? Type { get; set; }
        [Column("DeviceID")]
        [StringLength(250)]
        public string DeviceId { get; set; }
        [StringLength(250)]
        public string DeviceName { get; set; }
    }
}
