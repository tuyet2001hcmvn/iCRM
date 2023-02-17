using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductPromotionModel", Schema = "Marketing")]
    public partial class ProductPromotionModel
    {
        [Key]
        public Guid ProductPromotionId { get; set; }
        public Guid? TargetGroupId { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(100)]
        public string ProductPromotionTitle { get; set; }
        public bool? IsSendCatalogue { get; set; }
        [StringLength(50)]
        public string SendTypeCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndTime { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
