using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Competitor_Industry_Mapping", Schema = "Customer")]
    public partial class CompetitorIndustryMapping
    {
        [Key]
        public Guid CompetitorIndustryId { get; set; }
        public Guid? CompetitorId { get; set; }
        [StringLength(50)]
        public string IndustryCode { get; set; }
    }
}
