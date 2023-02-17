using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerPromotionModel", Schema = "tMasterData")]
    public partial class CustomerPromotionModel
    {
        public CustomerPromotionModel()
        {
            CustomerPromotionProductModels = new HashSet<CustomerPromotionProductModel>();
        }

        [Key]
        public Guid PromotionId { get; set; }
        [Required]
        [StringLength(50)]
        public string PromotionCode { get; set; }
        [Required]
        [StringLength(100)]
        public string PromotionName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectToDate { get; set; }
        public string Description { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [StringLength(4000)]
        public string Notes { get; set; }

        [InverseProperty(nameof(CustomerPromotionProductModel.Promotion))]
        public virtual ICollection<CustomerPromotionProductModel> CustomerPromotionProductModels { get; set; }
    }
}
