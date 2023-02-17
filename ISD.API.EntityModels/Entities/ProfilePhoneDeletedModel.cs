using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfilePhoneDeletedModel", Schema = "Customer")]
    public partial class ProfilePhoneDeletedModel
    {
        [Key]
        public Guid PhoneId { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
