using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SaleProcessModel", Schema = "tMasterData")]
    public partial class SaleProcessModel
    {
        [Key]
        public Guid SaleProcessId { get; set; }
        [Required]
        [StringLength(100)]
        public string SaleProcessDescription { get; set; }
    }
}
