using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProvinceModel", Schema = "tMasterData")]
    public partial class ProvinceModel
    {
        [Key]
        public Guid ProvinceId { get; set; }
        [Required]
        [StringLength(200)]
        public string ProvinceName { get; set; }
        [StringLength(50)]
        public string ProvinceCode { get; set; }
        [StringLength(50)]
        public string Area { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        [StringLength(200)]
        public string ConfigPriceCode { get; set; }
        public bool? IsHasLicensePrice { get; set; }
        public bool? IsForeign { get; set; }
    }
}
