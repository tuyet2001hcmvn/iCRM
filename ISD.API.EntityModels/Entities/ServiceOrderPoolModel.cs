using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderPoolModel", Schema = "ghService")]
    public partial class ServiceOrderPoolModel
    {
        [Key]
        public Guid ServiceOrderPoolId { get; set; }
        [Required]
        [StringLength(100)]
        public string ServiceOrderPoolCode { get; set; }
        [Required]
        [StringLength(100)]
        public string ServiceOrderPoolName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
