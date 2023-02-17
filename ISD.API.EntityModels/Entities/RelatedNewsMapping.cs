using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RelatedNews_Mapping", Schema = "tMasterData")]
    public partial class RelatedNewsMapping
    {
        [Key]
        public Guid RelatedNewsId { get; set; }
        public Guid? News1 { get; set; }
        public Guid? News2 { get; set; }
    }
}
