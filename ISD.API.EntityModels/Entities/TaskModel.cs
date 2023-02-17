using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskModel", Schema = "Task")]
    [Index(nameof(CreateTime), Name = "index_v_CreateTime")]
    public partial class TaskModel
    {
        public TaskModel()
        {
            TaskFileMappings = new HashSet<TaskFileMapping>();
            TaskProductModels = new HashSet<TaskProductModel>();
        }

        [Key]
        public Guid TaskId { get; set; }
        public int TaskCode { get; set; }
        [Required]
        [StringLength(4000)]
        public string Summary { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        [StringLength(10)]
        public string PriorityCode { get; set; }
        public Guid WorkFlowId { get; set; }
        public Guid TaskStatusId { get; set; }
        public Guid? ProductWarrantyId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReceiveDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EstimateEndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public Guid CompanyId { get; set; }
        public Guid StoreId { get; set; }
        [StringLength(20)]
        public string Reporter { get; set; }
        [StringLength(50)]
        public string ServiceTechnicalTeamCode { get; set; }
        public string CustomerReviews { get; set; }
        public Guid? ConstructionUnit { get; set; }
        public Guid? ConstructionUnitContact { get; set; }
        [StringLength(200)]
        public string FileUrl { get; set; }
        [StringLength(10)]
        public string CommonMistakeCode { get; set; }
        public int? DateKey { get; set; }
        [StringLength(50)]
        public string ErrorTypeCode { get; set; }
        [StringLength(50)]
        public string ErrorCode { get; set; }
        [Column("isRequiredCheckin")]
        public bool? IsRequiredCheckin { get; set; }
        [StringLength(4000)]
        public string VisitAddress { get; set; }
        [Column("lat")]
        [StringLength(200)]
        public string Lat { get; set; }
        [Column("lng")]
        [StringLength(200)]
        public string Lng { get; set; }
        [StringLength(50)]
        public string VisitSaleOfficeCode { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(50)]
        public string VisitTypeCode { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        public string CancelReason { get; set; }
        public Guid? DeleteBy { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [Column("isPrivate")]
        public bool? IsPrivate { get; set; }
        [Column("isRemind")]
        public bool? IsRemind { get; set; }
        public int? RemindTime { get; set; }
        [StringLength(50)]
        public string RemindCycle { get; set; }
        [Column("isRemindForReporter")]
        public bool? IsRemindForReporter { get; set; }
        [Column("isRemindForAssignee")]
        public bool? IsRemindForAssignee { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RemindStartDate { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Property6 { get; set; }
        public Guid? ParentTaskId { get; set; }
        [StringLength(50)]
        public string SalesSupervisorCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date1 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date3 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date5 { get; set; }
        [StringLength(500)]
        public string ProfileAddress { get; set; }
        public bool? IsAssignGroup { get; set; }
        public bool? IsTogether { get; set; }
        public bool? IsSync { get; set; }
        public int? TaskTempCode { get; set; }
        [StringLength(50)]
        public string SubtaskCode { get; set; }
        public string ShortNote { get; set; }
        [StringLength(500)]
        public string VisitPlace { get; set; }
        public Guid? GoogleEventId { get; set; }
        public bool? HasSurvey { get; set; }

        [InverseProperty(nameof(TaskFileMapping.Task))]
        public virtual ICollection<TaskFileMapping> TaskFileMappings { get; set; }
        [InverseProperty(nameof(TaskProductModel.Task))]
        public virtual ICollection<TaskProductModel> TaskProductModels { get; set; }
    }
}
