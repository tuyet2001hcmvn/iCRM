using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ResourceModel", Schema = "pms")]
    public partial class ResourceModel
    {
        [Key]
        [StringLength(400)]
        public string ResourceKey { get; set; }
        [StringLength(1000)]
        public string ResourceValue { get; set; }
        [StringLength(4000)]
        public string ResourceComment { get; set; }
    }
}
