using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkFlowModel", Schema = "Task")]
    public partial class WorkFlowModel
    {
        public WorkFlowModel()
        {
            StatusTransitionModels = new HashSet<StatusTransitionModel>();
            TaskStatusModels = new HashSet<TaskStatusModel>();
        }

        [Key]
        public Guid WorkFlowId { get; set; }
        [StringLength(50)]
        public string WorkFlowCode { get; set; }
        [Required]
        [StringLength(200)]
        public string WorkFlowName { get; set; }
        [StringLength(200)]
        public string ImageUrl { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        [StringLength(50)]
        public string WorkflowCategoryCode { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        public bool? IsDisabledSummary { get; set; }

        [InverseProperty(nameof(StatusTransitionModel.WorkFlow))]
        public virtual ICollection<StatusTransitionModel> StatusTransitionModels { get; set; }
        [InverseProperty(nameof(TaskStatusModel.WorkFlow))]
        public virtual ICollection<TaskStatusModel> TaskStatusModels { get; set; }
    }
}
