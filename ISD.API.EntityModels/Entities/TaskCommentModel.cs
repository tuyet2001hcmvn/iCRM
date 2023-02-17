using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskCommentModel", Schema = "Task")]
    public partial class TaskCommentModel
    {
        public TaskCommentModel()
        {
            CommentFileMappings = new HashSet<CommentFileMapping>();
        }

        [Key]
        public Guid TaskCommentId { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(4000)]
        public string Comment { get; set; }
        [StringLength(20)]
        public string ProcessUser { get; set; }
        public Guid? FromStatusId { get; set; }
        public Guid? ToStatusId { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [InverseProperty(nameof(CommentFileMapping.TaskComment))]
        public virtual ICollection<CommentFileMapping> CommentFileMappings { get; set; }
    }
}
