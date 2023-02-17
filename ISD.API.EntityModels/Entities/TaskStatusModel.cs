using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskStatusModel", Schema = "Task")]
    public partial class TaskStatusModel
    {
        public TaskStatusModel()
        {
            KanbanTaskStatusMappings = new HashSet<KanbanTaskStatusMapping>();
            StatusTransitionModelFromStatuses = new HashSet<StatusTransitionModel>();
            StatusTransitionModelToStatuses = new HashSet<StatusTransitionModel>();
        }

        [Key]
        public Guid TaskStatusId { get; set; }
        [Required]
        [StringLength(200)]
        public string TaskStatusName { get; set; }
        public Guid? WorkFlowId { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        [StringLength(50)]
        public string TaskStatusCode { get; set; }
        [StringLength(50)]
        public string ProcessCode { get; set; }
        [StringLength(200)]
        public string Category { get; set; }
        public int? PositionLeft { get; set; }
        public int? PositionRight { get; set; }
        public bool? AutoUpdateEndDate { get; set; }

        [ForeignKey(nameof(WorkFlowId))]
        [InverseProperty(nameof(WorkFlowModel.TaskStatusModels))]
        public virtual WorkFlowModel WorkFlow { get; set; }
        [InverseProperty(nameof(KanbanTaskStatusMapping.TaskStatus))]
        public virtual ICollection<KanbanTaskStatusMapping> KanbanTaskStatusMappings { get; set; }
        [InverseProperty(nameof(StatusTransitionModel.FromStatus))]
        public virtual ICollection<StatusTransitionModel> StatusTransitionModelFromStatuses { get; set; }
        [InverseProperty(nameof(StatusTransitionModel.ToStatus))]
        public virtual ICollection<StatusTransitionModel> StatusTransitionModelToStatuses { get; set; }
    }
}
