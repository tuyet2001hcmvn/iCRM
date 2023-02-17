using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderDetailAccessoryModel", Schema = "ghService")]
    public partial class ServiceOrderDetailAccessoryModel
    {
        [Key]
        public Guid ServiceOrderDetailAccessoryId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public Guid? FixingTypeId { get; set; }
        [StringLength(50)]
        public string MaterialNumber { get; set; }
        [StringLength(200)]
        public string ShortText { get; set; }
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Total { get; set; }
        [StringLength(50)]
        public string AccessoryCode { get; set; }
        [StringLength(200)]
        public string AccessoryName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AccessoryPrice { get; set; }
        [StringLength(200)]
        public string WarehouseCode { get; set; }
        [StringLength(200)]
        public string Unit { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        public bool? UrgentServiceOrder { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        [StringLength(50)]
        public string Plant { get; set; }
        [StringLength(200)]
        public string WarehouseName { get; set; }
        public int? Number { get; set; }

        [ForeignKey(nameof(ServiceOrderId))]
        [InverseProperty(nameof(ServiceOrderModel.ServiceOrderDetailAccessoryModels))]
        public virtual ServiceOrderModel ServiceOrder { get; set; }
    }
}
