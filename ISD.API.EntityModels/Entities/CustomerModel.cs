using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerModel", Schema = "ghMasterData")]
    [Index(nameof(Phone), Name = "UC_Phone", IsUnique = true)]
    public partial class CustomerModel
    {
        public CustomerModel()
        {
            AccessorySaleOrderModels = new HashSet<AccessorySaleOrderModel>();
            CustomerGiftDetailModels = new HashSet<CustomerGiftDetailModel>();
            SaleOrderMasterModels = new HashSet<SaleOrderMasterModel>();
            ServiceOrderModels = new HashSet<ServiceOrderModel>();
        }

        [Key]
        public Guid CustomerId { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        public int? CustomerType { get; set; }
        [StringLength(1000)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string IdentityNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IdentityDate { get; set; }
        [StringLength(100)]
        public string IdentityPlace { get; set; }
        public bool? Gender { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(2000)]
        public string CustomerAddress { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Phone2 { get; set; }
        [StringLength(200)]
        public string EmailAddress { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        public Guid? Career { get; set; }
        [StringLength(100)]
        public string TaxCode { get; set; }
        [StringLength(200)]
        public string IdentityUrl { get; set; }
        [StringLength(50)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int GeneratedCode { get; set; }
        public bool? Actived { get; set; }
        [StringLength(200)]
        public string IdentityUrl2 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Discount { get; set; }

        [InverseProperty(nameof(AccessorySaleOrderModel.Customer))]
        public virtual ICollection<AccessorySaleOrderModel> AccessorySaleOrderModels { get; set; }
        [InverseProperty(nameof(CustomerGiftDetailModel.Customer))]
        public virtual ICollection<CustomerGiftDetailModel> CustomerGiftDetailModels { get; set; }
        [InverseProperty(nameof(SaleOrderMasterModel.Customer))]
        public virtual ICollection<SaleOrderMasterModel> SaleOrderMasterModels { get; set; }
        [InverseProperty(nameof(ServiceOrderModel.Customer))]
        public virtual ICollection<ServiceOrderModel> ServiceOrderModels { get; set; }
    }
}
