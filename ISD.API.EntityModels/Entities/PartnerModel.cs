using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PartnerModel", Schema = "Customer")]
    public partial class PartnerModel
    {
        [Key]
        public Guid PartnerId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string PartnerType { get; set; }
        public Guid? PartnerProfileId { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(PartnerProfileId))]
        [InverseProperty(nameof(ProfileModel.PartnerModelPartnerProfiles))]
        public virtual ProfileModel PartnerProfile { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.PartnerModelProfiles))]
        public virtual ProfileModel Profile { get; set; }
    }
}
