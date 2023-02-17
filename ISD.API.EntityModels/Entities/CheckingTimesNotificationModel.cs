using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CheckingTimesNotificationModel", Schema = "ghService")]
    public partial class CheckingTimesNotificationModel
    {
        [Key]
        public Guid CheckingTimesId { get; set; }
        public Guid? AccountId { get; set; }
        [StringLength(10)]
        public string CustomerCode { get; set; }
        [StringLength(4000)]
        public string CheckingTimesDescription { get; set; }
        [Column("isRead")]
        public bool? IsRead { get; set; }
    }
}
