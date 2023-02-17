using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ConfigurationModel", Schema = "tSale")]
    public partial class ConfigurationModel
    {
        [Key]
        public Guid ConfigurationId { get; set; }
        [Required]
        [StringLength(50)]
        public string ConfigurationCode { get; set; }
        [Required]
        [StringLength(100)]
        public string ConfigurationName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
