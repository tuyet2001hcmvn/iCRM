using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileDeletedModel", Schema = "Customer")]
    public partial class ProfileDeletedModel
    {
        [Key]
        public Guid ProfileId { get; set; }
        public int ProfileCode { get; set; }
        [StringLength(20)]
        public string ProfileForeignCode { get; set; }
        [Column("isForeignCustomer")]
        public bool? IsForeignCustomer { get; set; }
        [StringLength(20)]
        public string CustomerTypeCode { get; set; }
        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(255)]
        public string ProfileName { get; set; }
        [StringLength(255)]
        public string ProfileShortName { get; set; }
        [StringLength(50)]
        public string AbbreviatedName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        [StringLength(10)]
        public string Age { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [Column("SAPPhone")]
        [StringLength(100)]
        public string Sapphone { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        [StringLength(10)]
        public string CountryCode { get; set; }
        [StringLength(10)]
        public string SaleOfficeCode { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VisitDate { get; set; }
        public bool? Actived { get; set; }
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        [StringLength(20)]
        public string CreateByEmployee { get; set; }
        [StringLength(50)]
        public string CreateAtCompany { get; set; }
        [StringLength(50)]
        public string CreateAtSaleOrg { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(50)]
        public string CustomerAccountGroupCode { get; set; }
        [StringLength(50)]
        public string CustomerGroupCode { get; set; }
        [StringLength(50)]
        public string PaymentTermCode { get; set; }
        [StringLength(50)]
        public string CustomerAccountAssignmentGroupCode { get; set; }
        [StringLength(50)]
        public string CashMgmtGroupCode { get; set; }
        [StringLength(50)]
        public string ReconcileAccountCode { get; set; }
        [StringLength(50)]
        public string CustomerSourceCode { get; set; }
        [StringLength(50)]
        public string TaxNo { get; set; }
        [StringLength(50)]
        public string Website { get; set; }
        [StringLength(50)]
        public string CustomerCareerCode { get; set; }
        [StringLength(50)]
        public string CompanyNumber { get; set; }
        [StringLength(50)]
        public string AddressTypeCode { get; set; }
        public int? ProjectCode { get; set; }
        [StringLength(50)]
        public string ProjectStatusCode { get; set; }
        [StringLength(50)]
        public string QualificationLevelCode { get; set; }
        [StringLength(50)]
        public string ProjectSourceCode { get; set; }
        public Guid? ReferenceProfileId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ContractValue { get; set; }
        [StringLength(500)]
        public string Text1 { get; set; }
        [StringLength(500)]
        public string Text2 { get; set; }
        [StringLength(500)]
        public string Text3 { get; set; }
        [StringLength(500)]
        public string Text4 { get; set; }
        [StringLength(500)]
        public string Text5 { get; set; }
        [StringLength(50)]
        public string Dropdownlist1 { get; set; }
        [StringLength(50)]
        public string Dropdownlist2 { get; set; }
        [StringLength(50)]
        public string Dropdownlist3 { get; set; }
        [StringLength(50)]
        public string Dropdownlist4 { get; set; }
        [StringLength(50)]
        public string Dropdownlist5 { get; set; }
        [StringLength(50)]
        public string Dropdownlist6 { get; set; }
        [StringLength(50)]
        public string Dropdownlist7 { get; set; }
        [StringLength(50)]
        public string Dropdownlist8 { get; set; }
        [StringLength(50)]
        public string Dropdownlist9 { get; set; }
        [StringLength(50)]
        public string Dropdownlist10 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date1 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date3 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date5 { get; set; }
        [StringLength(4000)]
        public string ProjectLocation { get; set; }
        public bool? IsAnCuongAccessory { get; set; }
        [StringLength(200)]
        public string Laminate { get; set; }
        [Column("MFC")]
        [StringLength(200)]
        public string Mfc { get; set; }
        [StringLength(200)]
        public string Veneer { get; set; }
        [StringLength(200)]
        public string Flooring { get; set; }
        [StringLength(200)]
        public string Accessories { get; set; }
        [StringLength(200)]
        public string KitchenEquipment { get; set; }
        [StringLength(200)]
        public string OtherBrand { get; set; }
        [StringLength(200)]
        public string HandoverFurniture { get; set; }
    }
}
