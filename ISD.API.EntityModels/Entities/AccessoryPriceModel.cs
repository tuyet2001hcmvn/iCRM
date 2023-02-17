using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessoryPriceModel", Schema = "ghMasterData")]
    public partial class AccessoryPriceModel
    {
        [Key]
        public Guid AccessoryPriceId { get; set; }
        [Column("SAPCode")]
        [StringLength(50)]
        public string Sapcode { get; set; }
        [StringLength(50)]
        public string AccessoryCode { get; set; }
        [StringLength(40)]
        public string ConditionType { get; set; }
        [StringLength(10)]
        public string PriceGroup { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? UnitPrice { get; set; }
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectedToDate { get; set; }
        public bool? Actived { get; set; }
    }
}
