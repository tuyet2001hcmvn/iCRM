using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("ProfileTempModel", Schema = "Customer")]
    public partial class ProfileTempModel
    {
        [StringLength(1000)]
        public string ProfileCode { get; set; }
        [StringLength(1000)]
        public string ProfileName { get; set; }
        [StringLength(4000)]
        public string FullName { get; set; }
        [StringLength(1000)]
        public string ShortName { get; set; }
        [StringLength(1000)]
        public string ForeignCode { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        [StringLength(1000)]
        public string Mobile { get; set; }
        [StringLength(1000)]
        public string Phone { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string CustomerTypeCode { get; set; }
        [StringLength(1000)]
        public string ProductCareerCode { get; set; }
        [Column("isForeignCustomer")]
        public bool? IsForeignCustomer { get; set; }
        [StringLength(1000)]
        public string Role { get; set; }
        [StringLength(1000)]
        public string Country { get; set; }
        [StringLength(1000)]
        public string District { get; set; }
        public Guid? DistrictId { get; set; }
        [StringLength(1000)]
        public string TaxNo { get; set; }
        [StringLength(1000)]
        public string CustomerGroupCode { get; set; }
        [StringLength(1000)]
        public string SaleOffice { get; set; }
        [StringLength(1000)]
        public string SaleOfficeCode { get; set; }
        [StringLength(1000)]
        public string Province { get; set; }
        public Guid? ProvinceId { get; set; }
        [StringLength(1000)]
        public string SaleOrg { get; set; }
        [StringLength(1000)]
        public string Contact { get; set; }
        [StringLength(1000)]
        public string ContactName { get; set; }
        [StringLength(1000)]
        public string ContactMobile { get; set; }
        [StringLength(1000)]
        public string ContactPhone { get; set; }
        [StringLength(1000)]
        public string ContactEmail { get; set; }
        [StringLength(1000)]
        public string DepartmentCode { get; set; }
        [StringLength(1000)]
        public string Position { get; set; }
        [StringLength(1000)]
        public string Age { get; set; }
        [StringLength(1000)]
        public string PartyRole { get; set; }
        [StringLength(1000)]
        public string EmployeeCode { get; set; }
        [StringLength(1000)]
        public string BusinessPartner { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        [StringLength(1000)]
        public string RelationshipName { get; set; }
        public int? RowNumber { get; set; }
    }
}
