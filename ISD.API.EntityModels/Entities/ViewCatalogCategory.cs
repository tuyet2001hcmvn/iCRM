using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewCatalogCategory
    {
        public Guid CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        public int? OrderIndex { get; set; }
    }
}
