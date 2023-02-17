using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Task_File_Mapping", Schema = "Task")]
    public partial class TaskFileMapping
    {
        [Key]
        public Guid TaskId { get; set; }
        [Key]
        public Guid FileAttachmentId { get; set; }
        [StringLength(200)]
        public string Note { get; set; }

        [ForeignKey(nameof(FileAttachmentId))]
        [InverseProperty(nameof(FileAttachmentModel.TaskFileMappings))]
        public virtual FileAttachmentModel FileAttachment { get; set; }
        [ForeignKey(nameof(TaskId))]
        [InverseProperty(nameof(TaskModel.TaskFileMappings))]
        public virtual TaskModel Task { get; set; }
    }
}
