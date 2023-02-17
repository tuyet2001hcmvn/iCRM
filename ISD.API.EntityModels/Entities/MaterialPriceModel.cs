using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialPriceModel", Schema = "ghMasterData")]
    public partial class MaterialPriceModel
    {
        [Key]
        public Guid MaterialPriceId { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? InvoicePrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RegistrationFeePrice { get; set; }
        public Guid? DistrictId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedToDate { get; set; }

        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.MaterialPriceModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
    }
}
