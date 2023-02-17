using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PromotionByStoreModel", Schema = "ghMasterData")]
    public partial class PromotionByStoreModel
    {
        [Key]
        public Guid PromotionId { get; set; }
        [Key]
        public Guid StoreId { get; set; }

        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.PromotionByStoreModels))]
        public virtual StoreModel Store { get; set; }
    }
}
