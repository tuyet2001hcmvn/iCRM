using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("KanbanModel", Schema = "tMasterData")]
    public partial class KanbanModel
    {
        public KanbanModel()
        {
            KanbanDetailModels = new HashSet<KanbanDetailModel>();
        }

        [Key]
        public Guid KanbanId { get; set; }
        [StringLength(20)]
        public string KanbanCode { get; set; }
        [StringLength(200)]
        public string KanbanName { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public int? OrderIndex { get; set; }

        [InverseProperty(nameof(KanbanDetailModel.Kanban))]
        public virtual ICollection<KanbanDetailModel> KanbanDetailModels { get; set; }
    }
}
