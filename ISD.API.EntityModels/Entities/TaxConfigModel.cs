using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaxConfigModel", Schema = "ghMasterData")]
    public partial class TaxConfigModel
    {
        [Key]
        public Guid TaxId { get; set; }
        [StringLength(50)]
        public string TaxCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Tax { get; set; }
        public bool? Actived { get; set; }
    }
}
