using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProductProductInfo
    {
        public Guid? MaterialId { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        [Required]
        [StringLength(4)]
        public string CompanyCode { get; set; }
        [Column("SPECType")]
        [StringLength(50)]
        public string Spectype { get; set; }
    }
}
