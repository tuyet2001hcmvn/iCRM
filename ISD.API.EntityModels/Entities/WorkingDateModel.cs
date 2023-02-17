using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkingDateModel", Schema = "ghService")]
    public partial class WorkingDateModel
    {
        [Key]
        public Guid WorkingDateId { get; set; }
        public Guid StoreId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DayOff { get; set; }
    }
}
