using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialInvoicePriceModel", Schema = "ghMasterData")]
    public partial class MaterialInvoicePriceModel
    {
        [Key]
        public Guid MaterialPriceId { get; set; }
        [Column("SAPCode")]
        [StringLength(50)]
        public string Sapcode { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? InvoicePrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedToDate { get; set; }
        public bool? Actived { get; set; }
        [StringLength(50)]
        public string ProvinceCode { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }

        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.MaterialInvoicePriceModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
    }
}
