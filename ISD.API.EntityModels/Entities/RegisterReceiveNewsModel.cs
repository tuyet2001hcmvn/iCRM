using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RegisterReceiveNewsModel", Schema = "Customer")]
    public partial class RegisterReceiveNewsModel
    {
        [Key]
        public Guid RegisterReceiveNewsId { get; set; }
        [StringLength(4)]
        public string CompanyCode { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        public Guid? ProvinceId { get; set; }
        [StringLength(255)]
        public string ProvinceName { get; set; }
        public Guid? DistrictId { get; set; }
        [StringLength(255)]
        public string DistrictName { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
    }
}
