using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileLevelModel", Schema = "Customer")]
    public partial class ProfileLevelModel
    {
        [Key]
        public Guid CustomerLevelId { get; set; }
        [StringLength(20)]
        public string CustomerLevelCode { get; set; }
        [StringLength(200)]
        public string CustomerLevelName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? LineOfLevel { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ExchangeValue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ToDate { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
