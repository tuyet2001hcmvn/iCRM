using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileEmailModel", Schema = "Customer")]
    public partial class ProfileEmailModel
    {
        [Key]
        public Guid EmailId { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        public Guid? ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileEmailModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
