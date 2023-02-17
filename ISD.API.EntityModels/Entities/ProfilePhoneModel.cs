using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfilePhoneModel", Schema = "Customer")]
    public partial class ProfilePhoneModel
    {
        [Key]
        public Guid PhoneId { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        public Guid? ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfilePhoneModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
