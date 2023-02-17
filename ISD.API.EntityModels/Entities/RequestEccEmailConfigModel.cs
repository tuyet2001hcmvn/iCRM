using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RequestEccEmailConfigModel", Schema = "tMasterData")]
    public partial class RequestEccEmailConfigModel
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string ToEmail { get; set; }
        [StringLength(255)]
        public string Subject { get; set; }
        public string EmailContent { get; set; }
        [StringLength(50)]
        public string FromEmail { get; set; }
        [StringLength(20)]
        public string FromEmailPassword { get; set; }
        [StringLength(50)]
        public string Host { get; set; }
        public int? Port { get; set; }
    }
}
