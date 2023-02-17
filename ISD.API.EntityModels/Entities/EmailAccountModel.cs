using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("EmailAccountModel", Schema = "Marketing")]
    public partial class EmailAccountModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? ServerProviderId { get; set; }
        [Required]
        public string SenderName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        [StringLength(50)]
        public string Domain { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
        public bool? EnableSsl { get; set; }
        [Required]
        public string Signature { get; set; }
        public bool? IsSender { get; set; }
    }
}
