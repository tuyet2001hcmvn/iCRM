using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderDetailServiceModel", Schema = "ghService")]
    public partial class ServiceOrderDetailServiceModel
    {
        [Key]
        public Guid ServiceOrderDetailServiceId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public Guid? FixingTypeId { get; set; }
        [StringLength(50)]
        public string MaterialNumber { get; set; }
        [StringLength(200)]
        public string ShortText { get; set; }
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? HourPrice { get; set; }
        public int? Discount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Total { get; set; }
        [StringLength(50)]
        public string ServiceTypeCode { get; set; }
        [StringLength(50)]
        public string AccessoryCode { get; set; }
        [StringLength(50)]
        public string AccessoryCodeReference { get; set; }
        public Guid? AccessoryIdReference { get; set; }
        [StringLength(200)]
        public string AccessoryName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AccessoryPrice { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        [StringLength(50)]
        public string Plant { get; set; }
        public int? Number { get; set; }

        [ForeignKey(nameof(ServiceOrderId))]
        [InverseProperty(nameof(ServiceOrderModel.ServiceOrderDetailServiceModels))]
        public virtual ServiceOrderModel ServiceOrder { get; set; }
        [ForeignKey(nameof(ServiceTypeCode))]
        [InverseProperty(nameof(ServiceTypeModel.ServiceOrderDetailServiceModels))]
        public virtual ServiceTypeModel ServiceTypeCodeNavigation { get; set; }
    }
}
