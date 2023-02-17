using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialMinMaxPriceBySaleOrgModel", Schema = "ghMasterData")]
    public partial class MaterialMinMaxPriceBySaleOrgModel
    {
        [Key]
        public Guid MaterialMinMaxPriceId { get; set; }
        [Key]
        public Guid StoreId { get; set; }

        [ForeignKey(nameof(MaterialMinMaxPriceId))]
        [InverseProperty(nameof(MaterialMinMaxPriceModel.MaterialMinMaxPriceBySaleOrgModels))]
        public virtual MaterialMinMaxPriceModel MaterialMinMaxPrice { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.MaterialMinMaxPriceBySaleOrgModels))]
        public virtual StoreModel Store { get; set; }
    }
}
