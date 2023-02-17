using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SMSModel", Schema = "Customer")]
    public partial class Smsmodel
    {
        [Key]
        [Column("SMSId")]
        public Guid Smsid { get; set; }
        [Column("SMSContent")]
        [StringLength(4000)]
        public string Smscontent { get; set; }
        [Column("SMSTo")]
        [StringLength(50)]
        public string Smsto { get; set; }
        [StringLength(4000)]
        public string ResponseText { get; set; }
        [Column("isSent")]
        public bool? IsSent { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(100)]
        public string BrandName { get; set; }
        [StringLength(200)]
        public string ErrorMessage { get; set; }
    }
}
