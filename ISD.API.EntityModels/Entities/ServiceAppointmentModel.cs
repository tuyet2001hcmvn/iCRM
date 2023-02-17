using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceAppointmentModel", Schema = "ghService")]
    public partial class ServiceAppointmentModel
    {
        [Key]
        public Guid AppointmentId { get; set; }
        [StringLength(1000)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string SaleOrgCode { get; set; }
        [StringLength(50)]
        public string ServiceTypeCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? AppointmentTime { get; set; }
        [StringLength(500)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [StringLength(50)]
        public string LastModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LasModifiedTime { get; set; }
    }
}
