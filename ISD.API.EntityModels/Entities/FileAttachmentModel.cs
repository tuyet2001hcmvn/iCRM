using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("FileAttachmentModel", Schema = "Customer")]
    public partial class FileAttachmentModel
    {
        public FileAttachmentModel()
        {
            CommentFileMappings = new HashSet<CommentFileMapping>();
            ProfileFileMappings = new HashSet<ProfileFileMapping>();
            TaskFileMappings = new HashSet<TaskFileMapping>();
        }

        [Key]
        public Guid FileAttachmentId { get; set; }
        public Guid ObjectId { get; set; }
        [Required]
        [StringLength(50)]
        public string FileAttachmentCode { get; set; }
        [StringLength(255)]
        public string FileAttachmentName { get; set; }
        [StringLength(50)]
        public string FileExtention { get; set; }
        [StringLength(500)]
        public string FileUrl { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(50)]
        public string FileType { get; set; }

        [InverseProperty(nameof(CommentFileMapping.FileAttachment))]
        public virtual ICollection<CommentFileMapping> CommentFileMappings { get; set; }
        [InverseProperty(nameof(ProfileFileMapping.FileAttachment))]
        public virtual ICollection<ProfileFileMapping> ProfileFileMappings { get; set; }
        [InverseProperty(nameof(TaskFileMapping.FileAttachment))]
        public virtual ICollection<TaskFileMapping> TaskFileMappings { get; set; }
    }
}
