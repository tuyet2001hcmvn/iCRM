using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileEmailDeletedModel", Schema = "Customer")]
    public partial class ProfileEmailDeletedModel
    {
        [Key]
        public Guid EmailId { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
