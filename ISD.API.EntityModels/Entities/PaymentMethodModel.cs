using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PaymentMethodModel", Schema = "ghMasterData")]
    public partial class PaymentMethodModel
    {
        [Key]
        public Guid PaymentMethodId { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethodCode { get; set; }
        [Required]
        [StringLength(100)]
        public string PaymentMethodAccount { get; set; }
        public int? PaymentMethodType { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }
        [StringLength(100)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [StringLength(100)]
        public string LastModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedTime { get; set; }
    }
}
