using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CollectingAuthorityModel", Schema = "ghMasterData")]
    public partial class CollectingAuthorityModel
    {
        [Key]
        public Guid CollectingAuthorityId { get; set; }
        [StringLength(50)]
        public string CollectingAuthorityCode { get; set; }
        [StringLength(200)]
        public string CollectingAuthorityName { get; set; }
        public bool? Actived { get; set; }
    }
}
