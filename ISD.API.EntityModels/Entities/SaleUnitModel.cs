using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SaleUnitModel", Schema = "tMasterData")]
    public partial class SaleUnitModel
    {
        [Key]
        public Guid SaleUnitId { get; set; }
        [Required]
        [StringLength(100)]
        public string SaleUnitDescription { get; set; }
    }
}
