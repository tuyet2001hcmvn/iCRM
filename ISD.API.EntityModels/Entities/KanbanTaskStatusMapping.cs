using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Kanban_TaskStatus_Mapping", Schema = "tMasterData")]
    public partial class KanbanTaskStatusMapping
    {
        [Key]
        public Guid KanbanDetailId { get; set; }
        [Key]
        public Guid TaskStatusId { get; set; }
        [StringLength(200)]
        public string Note { get; set; }

        [ForeignKey(nameof(KanbanDetailId))]
        [InverseProperty(nameof(KanbanDetailModel.KanbanTaskStatusMappings))]
        public virtual KanbanDetailModel KanbanDetail { get; set; }
        [ForeignKey(nameof(TaskStatusId))]
        [InverseProperty(nameof(TaskStatusModel.KanbanTaskStatusMappings))]
        public virtual TaskStatusModel TaskStatus { get; set; }
    }
}
