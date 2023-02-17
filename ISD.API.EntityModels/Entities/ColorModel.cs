using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ColorModel", Schema = "tSale")]
    public partial class ColorModel
    {
        [Key]
        public Guid ColorId { get; set; }
        [Required]
        [StringLength(50)]
        public string ColorCode { get; set; }
        [Required]
        [StringLength(50)]
        public string ColorShortName { get; set; }
        [Required]
        [StringLength(100)]
        public string ColorName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
