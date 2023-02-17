using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_Opportunity_CompetitorDetailModel", Schema = "Customer")]
    public partial class ProfileOpportunityCompetitorDetailModel
    {
        [Key]
        public Guid OpportunityCompetitorDetailId { get; set; }
        public Guid? OpportunityCompetitorId { get; set; }
        public Guid? CompetitorId { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
    }
}
