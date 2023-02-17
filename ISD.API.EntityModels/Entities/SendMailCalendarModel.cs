using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SendMailCalendarModel", Schema = "Marketing")]
    public partial class SendMailCalendarModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? ToProfileId { get; set; }
        [StringLength(2000)]
        public string ToEmail { get; set; }
        [StringLength(4000)]
        public string FullName { get; set; }
        public bool? IsSend { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SendTime { get; set; }
        [Column("isError")]
        public bool? IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string Param { get; set; }
        public bool? IsBounce { get; set; }
        public bool? IsOpened { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastOpenedTime { get; set; }
        [Column("isConfirm")]
        public bool? IsConfirm { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ConfirmTime { get; set; }
        [Column("isCheckin")]
        public bool? IsCheckin { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckinTime { get; set; }
        public int? NumberOfParticipant { get; set; }
    }
}
