using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ApplicationLog")]
    public partial class ApplicationLog
    {
        [Key]
        public Guid ApplicationLogId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PerformedAt { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        [Column("PerformedBy_AccountId")]
        public Guid? PerformedByAccountId { get; set; }

        [ForeignKey(nameof(PerformedByAccountId))]
        [InverseProperty(nameof(AccountModel.ApplicationLogs))]
        public virtual AccountModel PerformedByAccount { get; set; }
    }
}
