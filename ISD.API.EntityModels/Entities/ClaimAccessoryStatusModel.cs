using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ClaimAccessoryStatusModel", Schema = "ghService")]
    public partial class ClaimAccessoryStatusModel
    {
        [Key]
        public int StatusId { get; set; }
        [StringLength(100)]
        public string StatusName { get; set; }
    }
}
