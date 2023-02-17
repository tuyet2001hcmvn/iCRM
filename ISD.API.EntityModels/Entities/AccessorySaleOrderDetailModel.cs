using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessorySaleOrderDetailModel", Schema = "ghSale")]
    public partial class AccessorySaleOrderDetailModel
    {
        [Key]
        public Guid AccessorySaleOrderDetailId { get; set; }
        public Guid? AccessorySaleOrderId { get; set; }
        public Guid? AccessorySellTypeId { get; set; }
        [StringLength(50)]
        public string AccessoryCode { get; set; }
        [StringLength(400)]
        public string AccessoryName { get; set; }
        [StringLength(20)]
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Total { get; set; }
        [StringLength(50)]
        public string Plant { get; set; }
        [StringLength(50)]
        public string WarehouseCode { get; set; }
        [StringLength(50)]
        public string Location { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        [StringLength(200)]
        public string WarehouseName { get; set; }
        public int? Number { get; set; }

        [ForeignKey(nameof(AccessorySaleOrderId))]
        [InverseProperty(nameof(AccessorySaleOrderModel.AccessorySaleOrderDetailModels))]
        public virtual AccessorySaleOrderModel AccessorySaleOrder { get; set; }
    }
}
