using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("GH_NotificationModel", Schema = "ghMasterData")]
    public partial class GhNotificationModel
    {
        [Key]
        public Guid NotificationId { get; set; }
        [StringLength(200)]
        public string NotificationUrl { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [StringLength(400)]
        public string NotificationContent { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NotificationDateTime { get; set; }
        [Column("isComplete")]
        public bool? IsComplete { get; set; }
    }
}
