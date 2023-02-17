using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ContactDetailModel", Schema = "tMasterData")]
    public partial class ContactDetailModel
    {
        [Key]
        public Guid ContactDetailId { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string DisplayPhone { get; set; }
        public int? OrderIndex { get; set; }
    }
}
