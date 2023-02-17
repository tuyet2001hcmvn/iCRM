using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("DeliveryDetailTempModel", Schema = "Warehouse")]
    public partial class DeliveryDetailTempModel
    {
        [StringLength(50)]
        public string DeliveryCode { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? UnitPrice { get; set; }
    }
}
