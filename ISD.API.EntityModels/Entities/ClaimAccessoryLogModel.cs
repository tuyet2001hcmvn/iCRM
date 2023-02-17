using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ClaimAccessoryLogModel", Schema = "ghService")]
    public partial class ClaimAccessoryLogModel
    {
        [Key]
        public Guid ClaimAccessoryLogId { get; set; }
        public Guid ClaimAccessoryId { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [StringLength(50)]
        public string CreatedUser { get; set; }
    }
}
