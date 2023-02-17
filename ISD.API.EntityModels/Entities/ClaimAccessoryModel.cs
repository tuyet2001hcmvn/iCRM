using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ClaimAccessoryModel", Schema = "ghService")]
    public partial class ClaimAccessoryModel
    {
        [Key]
        public Guid ClaimAccessoryId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public Guid? ServiceOrderDetailAccessoryId { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SendDate { get; set; }
        [StringLength(100)]
        public string MeansOfSending { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
