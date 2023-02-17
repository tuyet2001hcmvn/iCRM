using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewFaceCheckIn
    {
        [Column("PersonID")]
        [StringLength(150)]
        public string PersonId { get; set; }
        [StringLength(250)]
        public string PersonName { get; set; }
        [StringLength(500)]
        public string Avatar { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateCheckIn { get; set; }
        public TimeSpan? FirstTimeCheckIn { get; set; }
        [StringLength(250)]
        public string DeviceCheckIn { get; set; }
        public int? Type { get; set; }
    }
}
