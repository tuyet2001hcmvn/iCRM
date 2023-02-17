using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Profile_Catalog_Mapping", Schema = "Customer")]
    public partial class ProfileCatalogMapping
    {
        [Key]
        public Guid ProfileCatalogId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string CatalogCode { get; set; }
        public Guid? CompetitorId { get; set; }
        public string MaterialCode { get; set; }
        [StringLength(50)]
        public string MappingType { get; set; }
    }
}
