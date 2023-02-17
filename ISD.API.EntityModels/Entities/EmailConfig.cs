using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("EmailConfig", Schema = "tMasterData")]
    public partial class EmailConfig
    {
        [Key]
        public Guid EmailConfigId { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(50)]
        public string SmtpServer { get; set; }
        [StringLength(10)]
        public string SmtpPort { get; set; }
        public bool? EnableSsl { get; set; }
        [StringLength(200)]
        public string SmtpMailFrom { get; set; }
        [StringLength(100)]
        public string SmtpUser { get; set; }
        [StringLength(100)]
        public string SmtpPassword { get; set; }
        [StringLength(100)]
        public string ToEmail { get; set; }
        [Column("CCMail")]
        [StringLength(100)]
        public string Ccmail { get; set; }
        [Column("BCCMail")]
        [StringLength(100)]
        public string Bccmail { get; set; }
        [StringLength(200)]
        public string EmailTitle { get; set; }
        public string EmailContent { get; set; }
    }
}
