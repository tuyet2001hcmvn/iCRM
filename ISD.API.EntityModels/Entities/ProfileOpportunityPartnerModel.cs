using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_Opportunity_PartnerModel", Schema = "Customer")]
    public partial class ProfileOpportunityPartnerModel
    {
        [Key]
        public Guid OpportunityPartnerId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? PartnerId { get; set; }
        [StringLength(50)]
        public string PartnerType { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public bool? IsMain { get; set; }
        [StringLength(50)]
        public string ConstructionCategory { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ProjectValue { get; set; }
        public bool? IsWon { get; set; }

        [ForeignKey(nameof(PartnerId))]
        [InverseProperty(nameof(ProfileModel.ProfileOpportunityPartnerModelPartners))]
        public virtual ProfileModel Partner { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileOpportunityPartnerModelProfiles))]
        public virtual ProfileModel Profile { get; set; }
    }
}
