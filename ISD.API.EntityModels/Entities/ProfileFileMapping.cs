using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_File_Mapping", Schema = "Customer")]
    public partial class ProfileFileMapping
    {
        [Key]
        public Guid ProfileId { get; set; }
        [Key]
        public Guid FileAttachmentId { get; set; }
        [StringLength(10)]
        public string Note { get; set; }

        [ForeignKey(nameof(FileAttachmentId))]
        [InverseProperty(nameof(FileAttachmentModel.ProfileFileMappings))]
        public virtual FileAttachmentModel FileAttachment { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileFileMappings))]
        public virtual ProfileModel Profile { get; set; }
    }
}
