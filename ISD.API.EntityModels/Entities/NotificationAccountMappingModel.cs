using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("NotificationAccountMappingModel", Schema = "ghMasterData")]
    public partial class NotificationAccountMappingModel
    {
        [Key]
        public Guid NotificationId { get; set; }
        [Key]
        public Guid AccountId { get; set; }
        public bool? IsRead { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(AccountModel.NotificationAccountMappingModels))]
        public virtual AccountModel Account { get; set; }
        [ForeignKey(nameof(NotificationId))]
        [InverseProperty(nameof(NotificationModel.NotificationAccountMappingModels))]
        public virtual NotificationModel Notification { get; set; }
    }
}
