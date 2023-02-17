using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AboutModel", Schema = "tMasterData")]
    public partial class AboutModel
    {
        [Key]
        public Guid AboutId { get; set; }
        [StringLength(100)]
        public string AboutTitle { get; set; }
        public string AboutDescription { get; set; }
    }
}
