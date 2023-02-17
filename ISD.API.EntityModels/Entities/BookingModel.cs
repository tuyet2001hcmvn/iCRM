using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("BookingModel", Schema = "ghService")]
    [Index(nameof(BookingCode), Name = "uc_BookingCode", IsUnique = true)]
    public partial class BookingModel
    {
        [Key]
        public Guid BookingModelId { get; set; }
        public long BookingCode { get; set; }
        public Guid? StoreId { get; set; }
        [StringLength(50)]
        public string Plate { get; set; }
        [StringLength(50)]
        public string BikeModel { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BookingDate { get; set; }
        public TimeSpan? BookingTime { get; set; }
        public Guid? CustomerId { get; set; }
        public bool? Active { get; set; }
        public bool? IsConfirm { get; set; }
        public bool? IsCreatedServiceOrder { get; set; }
    }
}
