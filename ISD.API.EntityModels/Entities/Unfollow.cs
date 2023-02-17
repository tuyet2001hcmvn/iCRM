using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Unfollow", Schema = "Marketing")]
    public partial class Unfollow
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
    }
}
