using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("KanbanDetailModel", Schema = "tMasterData")]
    public partial class KanbanDetailModel
    {
        public KanbanDetailModel()
        {
            KanbanTaskStatusMappings = new HashSet<KanbanTaskStatusMapping>();
        }

        [Key]
        public Guid KanbanDetailId { get; set; }
        public Guid? KanbanId { get; set; }
        [StringLength(200)]
        public string ColumnName { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(KanbanId))]
        [InverseProperty(nameof(KanbanModel.KanbanDetailModels))]
        public virtual KanbanModel Kanban { get; set; }
        [InverseProperty(nameof(KanbanTaskStatusMapping.KanbanDetail))]
        public virtual ICollection<KanbanTaskStatusMapping> KanbanTaskStatusMappings { get; set; }
    }
}
