using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialMinMaxPriceModel", Schema = "ghMasterData")]
    public partial class MaterialMinMaxPriceModel
    {
        public MaterialMinMaxPriceModel()
        {
            MaterialMinMaxPriceBySaleOrgModels = new HashSet<MaterialMinMaxPriceBySaleOrgModel>();
        }

        [Key]
        public Guid MaterialMinMaxPriceId { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MinPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MaxPrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedToDate { get; set; }
        [StringLength(100)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [StringLength(100)]
        public string LastModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedTime { get; set; }

        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.MaterialMinMaxPriceModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
        [InverseProperty(nameof(MaterialMinMaxPriceBySaleOrgModel.MaterialMinMaxPrice))]
        public virtual ICollection<MaterialMinMaxPriceBySaleOrgModel> MaterialMinMaxPriceBySaleOrgModels { get; set; }
    }
}
