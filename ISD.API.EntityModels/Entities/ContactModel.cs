using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ContactModel", Schema = "tMasterData")]
    public partial class ContactModel
    {
        [Key]
        public Guid ContactId { get; set; }
        [StringLength(4000)]
        public string ContactDescription { get; set; }
        [StringLength(4000)]
        public string ReviewDescription { get; set; }
    }
}
