using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("EmailTemplateConfigModel", Schema = "tMasterData")]
    public partial class EmailTemplateConfigModel
    {
        [Key]
        public Guid EmailTemplateConfigId { get; set; }
        [StringLength(50)]
        public string EmailTemplateType { get; set; }
        [StringLength(1000)]
        public string FromEmail { get; set; }
        [StringLength(1000)]
        public string ToEmail { get; set; }
        [StringLength(1000)]
        public string Subject { get; set; }
        [StringLength(4000)]
        public string Content { get; set; }
        [StringLength(1000)]
        public string EmailFrom { get; set; }
        [StringLength(1000)]
        public string EmailSender { get; set; }
        [StringLength(500)]
        public string EmailHost { get; set; }
        public int? EmailPort { get; set; }
        public bool? EmailEnableSsl { get; set; }
        [StringLength(500)]
        public string EmailAccount { get; set; }
        [StringLength(500)]
        public string EmailPassword { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
