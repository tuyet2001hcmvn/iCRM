using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_OpportunityStatus_Mapping", Schema = "Customer")]
    public partial class ProfileOpportunityStatusMapping
    {
        [Key]
        public Guid OpportunityStatusId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string CatalogCode { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [StringLength(50)]
        public string MappingType { get; set; }
    }
}
