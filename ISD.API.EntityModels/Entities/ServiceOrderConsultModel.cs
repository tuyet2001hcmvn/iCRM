using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderConsultModel", Schema = "ghService")]
    public partial class ServiceOrderConsultModel
    {
        [Key]
        public Guid ServiceOrderConsultId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        [StringLength(400)]
        public string RequestMessage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        public Guid? AccountId { get; set; }
        public bool? IsThayThe { get; set; }

        [ForeignKey(nameof(ServiceOrderId))]
        [InverseProperty(nameof(ServiceOrderModel.ServiceOrderConsultModels))]
        public virtual ServiceOrderModel ServiceOrder { get; set; }
    }
}
