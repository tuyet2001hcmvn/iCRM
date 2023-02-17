using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MailServerProviderModel", Schema = "Marketing")]
    public partial class MailServerProviderModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string IncomingHost { get; set; }
        public int? IncomingPort { get; set; }
        [Required]
        public string OutgoingHost { get; set; }
        public int OutgoingPort { get; set; }
    }
}
