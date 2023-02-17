using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Page_Module_Mapping", Schema = "pms")]
    public partial class PageModuleMapping
    {
        [Key]
        public Guid ModuleId { get; set; }
        [Key]
        public Guid PageId { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
