using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("LastRunCheckInOutModel")]
    public partial class LastRunCheckInOutModel
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastTime { get; set; }
    }
}
