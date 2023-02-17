using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StatusTransitionModel", Schema = "Task")]
    public partial class StatusTransitionModel
    {
        [Key]
        public Guid StatusTransitionId { get; set; }
        [StringLength(200)]
        public string TransitionName { get; set; }
        public string Description { get; set; }
        public Guid? WorkFlowId { get; set; }
        public Guid? FromStatusId { get; set; }
        public Guid? ToStatusId { get; set; }
        [Column("isAssigneePermission")]
        public bool? IsAssigneePermission { get; set; }
        [Column("isReporterPermission")]
        public bool? IsReporterPermission { get; set; }
        [StringLength(50)]
        public string Color { get; set; }
        [Column("isRequiredComment")]
        public bool? IsRequiredComment { get; set; }
        [Column("isCreateUserPermission")]
        public bool? IsCreateUserPermission { get; set; }
        [StringLength(20)]
        public string StatusTransitionIn { get; set; }
        [StringLength(20)]
        public string StatusTransitionOut { get; set; }
        [Column("isAutomaticTransitions")]
        public bool? IsAutomaticTransitions { get; set; }
        [StringLength(200)]
        public string BranchName { get; set; }
        public int? BranchPositionLeft { get; set; }
        public int? BranchPositionRight { get; set; }
        [StringLength(200)]
        public string BranchIn { get; set; }
        [StringLength(200)]
        public string BranchOut { get; set; }
        [Column("unsignedBranchName")]
        [StringLength(200)]
        public string UnsignedBranchName { get; set; }

        [ForeignKey(nameof(FromStatusId))]
        [InverseProperty(nameof(TaskStatusModel.StatusTransitionModelFromStatuses))]
        public virtual TaskStatusModel FromStatus { get; set; }
        [ForeignKey(nameof(ToStatusId))]
        [InverseProperty(nameof(TaskStatusModel.StatusTransitionModelToStatuses))]
        public virtual TaskStatusModel ToStatus { get; set; }
        [ForeignKey(nameof(WorkFlowId))]
        [InverseProperty(nameof(WorkFlowModel.StatusTransitionModels))]
        public virtual WorkFlowModel WorkFlow { get; set; }
    }
}
