using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SourceModel", Schema = "tMasterData")]
    public partial class SourceModel
    {
        [Key]
        public Guid SourceId { get; set; }
        [Required]
        [StringLength(100)]
        public string SourceDescription { get; set; }
    }
}
