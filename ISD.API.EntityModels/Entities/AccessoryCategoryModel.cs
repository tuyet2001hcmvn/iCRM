using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessoryCategoryModel", Schema = "tSale")]
    public partial class AccessoryCategoryModel
    {
        [Key]
        public Guid AccessoryCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string AccessoryCategoryCode { get; set; }
        [Required]
        [StringLength(100)]
        public string AccessoryCategoryName { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        public bool Actived { get; set; }
    }
}
