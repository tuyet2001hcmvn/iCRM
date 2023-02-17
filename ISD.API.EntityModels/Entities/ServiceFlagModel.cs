using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceFlagModel", Schema = "ghService")]
    public partial class ServiceFlagModel
    {
        [Key]
        public Guid ServiceFlagId { get; set; }
        [StringLength(50)]
        public string ServiceFlagCode { get; set; }
        public bool? IsTinhTien { get; set; }
        public bool? Actived { get; set; }
    }
}
