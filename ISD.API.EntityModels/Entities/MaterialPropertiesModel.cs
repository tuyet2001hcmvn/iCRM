using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialPropertiesModel", Schema = "ghMasterData")]
    public partial class MaterialPropertiesModel
    {
        [Key]
        public Guid PropertiesId { get; set; }
        [StringLength(40)]
        public string MaterialCode { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
    }
}
