using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskProductModel", Schema = "Task")]
    public partial class TaskProductModel
    {
        public TaskProductModel()
        {
            TaskProductAccessoryModels = new HashSet<TaskProductAccessoryModel>();
            TaskProductUsualErrorModels = new HashSet<TaskProductUsualErrorModel>();
        }

        [Key]
        public Guid TaskProductId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? ProductId { get; set; }
        public int? Qty { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(50)]
        public string WarrantyProductTypeCode { get; set; }
        [StringLength(50)]
        public string ErrorTypeCode { get; set; }
        [StringLength(50)]
        public string ErrorCode { get; set; }
        [StringLength(50)]
        public string ProductLevelCode { get; set; }
        [StringLength(50)]
        public string ProductColorCode { get; set; }
        [StringLength(50)]
        public string ProductCategoryCode { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        [Column("SAPSOWarranty")]
        public string Sapsowarranty { get; set; }
        [Column("SAPSOProduct")]
        public string Sapsoproduct { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ProductValue { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? WarrantyValue { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountValue { get; set; }
        [StringLength(100)]
        public string SerialNumber { get; set; }

        [ForeignKey(nameof(TaskId))]
        [InverseProperty(nameof(TaskModel.TaskProductModels))]
        public virtual TaskModel Task { get; set; }
        [InverseProperty(nameof(TaskProductAccessoryModel.TaskProduct))]
        public virtual ICollection<TaskProductAccessoryModel> TaskProductAccessoryModels { get; set; }
        [InverseProperty(nameof(TaskProductUsualErrorModel.TaskProduct))]
        public virtual ICollection<TaskProductUsualErrorModel> TaskProductUsualErrorModels { get; set; }
    }
}
