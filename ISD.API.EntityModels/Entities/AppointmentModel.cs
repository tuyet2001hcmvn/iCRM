using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AppointmentModel", Schema = "Task")]
    [Index(nameof(VisitDate), Name = "index_AppointmentModel_VisitDate")]
    public partial class AppointmentModel
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid? PrimaryContactId { get; set; }
        [StringLength(50)]
        public string CustomerClassCode { get; set; }
        [StringLength(50)]
        public string CategoryCode { get; set; }
        [StringLength(50)]
        public string ShowroomCode { get; set; }
        [StringLength(20)]
        public string SaleEmployeeCode { get; set; }
        [StringLength(50)]
        public string ChannelCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VisitDate { get; set; }
        [StringLength(4000)]
        public string Requirement { get; set; }
        [Column("isSentSMS")]
        public bool? IsSentSms { get; set; }
        [StringLength(4000)]
        public string SaleEmployeeOffer { get; set; }
        [Column("isVisitCabinetPro")]
        public bool? IsVisitCabinetPro { get; set; }
        public string VisitCabinetProRequest { get; set; }
    }
}
