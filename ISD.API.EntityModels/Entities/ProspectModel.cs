using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProspectModel", Schema = "ghMasterData")]
    public partial class ProspectModel
    {
        [Key]
        public Guid ProspectId { get; set; }
        [StringLength(50)]
        public string ProspectCode { get; set; }
        public int? ProspectType { get; set; }
        [StringLength(2000)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string IdentityNumber { get; set; }
        public bool? Gender { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(2000)]
        public string ProspectAddress { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Phone2 { get; set; }
        [StringLength(200)]
        public string EmailAddress { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string TaxCode { get; set; }
        public bool? IsOpportunity { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        public Guid? PurchasedTime { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? EstimatedRevenue { get; set; }
        public int? SuccessPercent { get; set; }
        [StringLength(50)]
        public string CreatedUser { get; set; }
        public Guid? SourceId { get; set; }
        [StringLength(200)]
        public string IdentityUrl { get; set; }
        [StringLength(100)]
        public string UsedMaterial { get; set; }
        [StringLength(100)]
        public string UsingMaterial { get; set; }
        public int GeneratedCode { get; set; }
        public bool? Actived { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IdentityDate { get; set; }
        [StringLength(100)]
        public string IdentityPlace { get; set; }
        public Guid? Career { get; set; }
    }
}
