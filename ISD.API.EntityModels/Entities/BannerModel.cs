using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("BannerModel", Schema = "tMasterData")]
    public partial class BannerModel
    {
        [Key]
        public Guid BannerId { get; set; }
        [Required]
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
    }
}
