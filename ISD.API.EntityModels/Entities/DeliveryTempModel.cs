using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("DeliveryTempModel", Schema = "Warehouse")]
    public partial class DeliveryTempModel
    {
        [StringLength(50)]
        public string TaskCode { get; set; }
        public int DeliveryCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DocumentDate { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
    }
}
