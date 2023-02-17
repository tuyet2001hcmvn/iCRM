using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("ProfileTemp2Model", Schema = "Customer")]
    public partial class ProfileTemp2Model
    {
        [StringLength(20)]
        public string ProfileCode { get; set; }
        [StringLength(20)]
        public string ProfileForeignCode { get; set; }
        [StringLength(50)]
        public string CustomerTypeCode { get; set; }
        [StringLength(4000)]
        public string ProfileName { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Mobile { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(50)]
        public string TaxNo { get; set; }
        [StringLength(50)]
        public string CreateAtSaleOrg { get; set; }
        [StringLength(50)]
        public string SaleOffice { get; set; }
        [StringLength(200)]
        public string ProvinceName { get; set; }
        [StringLength(200)]
        public string DistrictName { get; set; }
        [StringLength(200)]
        public string ProfileGroupName { get; set; }
        [StringLength(200)]
        public string ProfileCareerName { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        [StringLength(50)]
        public string Owner { get; set; }
        [StringLength(200)]
        public string Role { get; set; }
        [StringLength(50)]
        public string Actived { get; set; }
        [StringLength(50)]
        public string ContactCode { get; set; }
        [StringLength(4000)]
        public string ContactName { get; set; }
        [StringLength(50)]
        public string ContactMobile { get; set; }
        [StringLength(4000)]
        public string ContactAddress { get; set; }
        [StringLength(50)]
        public string ContactEmail { get; set; }
    }
}
