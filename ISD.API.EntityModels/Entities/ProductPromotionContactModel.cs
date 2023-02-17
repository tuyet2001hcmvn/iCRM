using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductPromotionContactModel", Schema = "Marketing")]
    public partial class ProductPromotionContactModel
    {
        [Key]
        public Guid ProductPromotionContactId { get; set; }
        public Guid? ProductPromotionDetailId { get; set; }
        public string ProfileAddress { get; set; }
        public Guid? ContactId { get; set; }
        [StringLength(100)]
        public string ContactName { get; set; }
        [StringLength(20)]
        public string ContactPhone { get; set; }
        public bool? CheckAddress { get; set; }
        public bool? CheckContact { get; set; }
    }
}
