using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CreditLimitModel", Schema = "Customer")]
    public partial class CreditLimitModel
    {
        [Key]
        public Guid CreditLimitId { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [StringLength(50)]
        public string SaleOrgCode { get; set; }
        [StringLength(20)]
        public string ProfileForeignCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ToDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Amount { get; set; }
    }
}
