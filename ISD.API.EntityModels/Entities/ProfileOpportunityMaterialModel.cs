using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_Opportunity_MaterialModel", Schema = "Customer")]
    public partial class ProfileOpportunityMaterialModel
    {
        [Key]
        public Guid OpportunityMaterialId { get; set; }
        public Guid? ReferenceId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        [StringLength(50)]
        public string MaterialType { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileOpportunityMaterialModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
