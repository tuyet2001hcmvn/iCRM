using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Comment_File_Mapping", Schema = "Task")]
    public partial class CommentFileMapping
    {
        [Key]
        public Guid TaskCommentId { get; set; }
        [Key]
        public Guid FileAttachmentId { get; set; }
        [StringLength(200)]
        public string Note { get; set; }

        [ForeignKey(nameof(FileAttachmentId))]
        [InverseProperty(nameof(FileAttachmentModel.CommentFileMappings))]
        public virtual FileAttachmentModel FileAttachment { get; set; }
        [ForeignKey(nameof(TaskCommentId))]
        [InverseProperty(nameof(TaskCommentModel.CommentFileMappings))]
        public virtual TaskCommentModel TaskComment { get; set; }
    }
}
