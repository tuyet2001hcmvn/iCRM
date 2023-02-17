using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PromotionBuyMaterialModel", Schema = "ghMasterData")]
    public partial class PromotionBuyMaterialModel
    {
        [Key]
        public Guid BuyMaterialId { get; set; }
        public Guid? PromotionId { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
    }
}
