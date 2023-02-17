using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("NotificationModel", Schema = "ghMasterData")]
    public partial class NotificationModel
    {
        public NotificationModel()
        {
            NotificationAccountMappingModels = new HashSet<NotificationAccountMappingModel>();
        }

        [Key]
        public Guid NotificationId { get; set; }
        [StringLength(160)]
        public string Title { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(4000)]
        public string Detail { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty(nameof(NotificationAccountMappingModel.Notification))]
        public virtual ICollection<NotificationAccountMappingModel> NotificationAccountMappingModels { get; set; }
    }
}
