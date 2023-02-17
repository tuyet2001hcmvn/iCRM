using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewFaceCheckOut
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
        public TimeSpan? LastTimeCheckIn { get; set; }
        [StringLength(250)]
        public string DeviceCheckOut { get; set; }
    }
}
