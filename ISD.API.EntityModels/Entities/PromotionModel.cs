using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PromotionModel", Schema = "tMasterData")]
    public partial class PromotionModel
    {
        public PromotionModel()
        {
            PromotionProductModels = new HashSet<PromotionProductModel>();
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

        [InverseProperty(nameof(PromotionProductModel.Promotion))]
        public virtual ICollection<PromotionProductModel> PromotionProductModels { get; set; }
    }
}
