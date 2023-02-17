using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_Opportunity_CompetitorModel", Schema = "Customer")]
    public partial class ProfileOpportunityCompetitorModel
    {
        [Key]
        public Guid OpportunityCompetitorId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string CatalogCode { get; set; }
        public string MaterialCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ProjectValue { get; set; }
        public bool? IsWon { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [StringLength(50)]
        public string MappingType { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileOpportunityCompetitorModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
