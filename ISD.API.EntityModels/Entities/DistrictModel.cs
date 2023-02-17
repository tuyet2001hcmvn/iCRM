using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("DistrictModel", Schema = "tMasterData")]
    public partial class DistrictModel
    {
        [Key]
        public Guid DistrictId { get; set; }
        public Guid ProvinceId { get; set; }
        [Required]
        [StringLength(50)]
        public string Appellation { get; set; }
        [Required]
        [StringLength(200)]
        public string DistrictName { get; set; }
        [StringLength(50)]
        public string DistrictCode { get; set; }
        [Column("RegisterVAT", TypeName = "decimal(18, 2)")]
        public decimal? RegisterVat { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        [StringLength(200)]
        public string VehicleRegistrationSignature { get; set; }
    }
}
