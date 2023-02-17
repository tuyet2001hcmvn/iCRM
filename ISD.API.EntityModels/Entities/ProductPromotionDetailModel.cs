using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductPromotionDetailModel", Schema = "Marketing")]
    public partial class ProductPromotionDetailModel
    {
        [Key]
        public Guid ProductPromotionDetailId { get; set; }
        public Guid? ProductPromotionId { get; set; }
        public Guid? ProfileId { get; set; }
        public bool? Status { get; set; }
    }
}
