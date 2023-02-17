using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerTastes_CollectionModel", Schema = "Customer")]
    public partial class CustomerTastesCollectionModel
    {
        [Key]
        public Guid CollectionId { get; set; }
        [StringLength(500)]
        public string CollectionCode { get; set; }
        [StringLength(500)]
        public string CollectionName { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? AppointmentId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
