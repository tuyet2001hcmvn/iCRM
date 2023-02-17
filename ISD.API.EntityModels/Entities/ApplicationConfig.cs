using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ApplicationConfig")]
    public partial class ApplicationConfig
    {
        [Key]
        [StringLength(100)]
        public string ConfigKey { get; set; }
        [StringLength(100)]
        public string ConfigValue { get; set; }
    }
}
