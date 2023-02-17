using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderModel", Schema = "ghService")]
    public partial class ServiceOrderModel
    {
        public ServiceOrderModel()
        {
            ServiceOrderConsultModels = new HashSet<ServiceOrderConsultModel>();
            ServiceOrderDetailAccessoryModels = new HashSet<ServiceOrderDetailAccessoryModel>();
            ServiceOrderDetailServiceModels = new HashSet<ServiceOrderDetailServiceModel>();
        }

        [Required]
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [Key]
        public Guid ServiceOrderId { get; set; }
        [StringLength(50)]
        public string ServiceOrderCode { get; set; }
        [StringLength(50)]
        public string ServiceOrderTypeCode { get; set; }
        [StringLength(100)]
        public string ServiceOrderName { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? SaleOrderMasterId { get; set; }
        public Guid? VehicleId { get; set; }
        [StringLength(4000)]
        public string CustomerRequest { get; set; }
        [StringLength(50)]
        public string KmDaDi { get; set; }
        public int? WashRequest { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public int? Number { get; set; }
        public Guid? AccountId1TiepNhan { get; set; }
        [StringLength(200)]
        public string SaleEmployee1Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Step1DateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Step1DateTimeDuKien { get; set; }
        [StringLength(4000)]
        public string Step1Note { get; set; }
        [StringLength(4000)]
        public string ConsultNote { get; set; }
        public Guid? AccountId1KyThuat1 { get; set; }
        [StringLength(200)]
        public string SaleEmployee1aName { get; set; }
        public Guid? AccountId1KyThuat2 { get; set; }
        [StringLength(200)]
        public string SaleEmployee1bName { get; set; }
        public Guid? AccountId2KiemTra { get; set; }
        [StringLength(200)]
        public string SaleEmployee2Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Step2DateTime { get; set; }
        [StringLength(50)]
        public string Step2HangMuc { get; set; }
        public Guid? Step2HangMucId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Step2NextDateTime { get; set; }
        [StringLength(50)]
        public string Step2Km { get; set; }
        public bool? Step2GetOldAccessory { get; set; }
        public Guid? Step3AccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Step3DateTime { get; set; }
        public Guid? AccountThuNgan { get; set; }
        public int? Step { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        [StringLength(500)]
        public string CustomerName { get; set; }
        [StringLength(50)]
        public string CustomerPhone { get; set; }
        [StringLength(200)]
        public string StoreName { get; set; }
        [StringLength(200)]
        public string Category { get; set; }
        [StringLength(200)]
        public string ServicePool { get; set; }
        [StringLength(200)]
        public string LicensePlate { get; set; }
        [StringLength(200)]
        public string SerialNumber { get; set; }
        [StringLength(200)]
        public string EngineNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BuyDate { get; set; }
        [StringLength(50)]
        public string ProfitCenterCode { get; set; }
        [StringLength(50)]
        public string ProductHierarchyCode { get; set; }
        [StringLength(50)]
        public string MaterialGroupCode { get; set; }
        [StringLength(50)]
        public string LaborCode { get; set; }
        [StringLength(50)]
        public string MaterialFreightGroupCode { get; set; }
        [StringLength(50)]
        public string ExternalMaterialGroupCode { get; set; }
        [StringLength(50)]
        public string TemperatureConditionCode { get; set; }
        [StringLength(50)]
        public string ContainerRequirementCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CurrentKilometers { get; set; }
        [StringLength(50)]
        public string PersonnalNumberId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Total { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        [Column("isUpdatedFromERP")]
        public bool? IsUpdatedFromErp { get; set; }
        [Column("UpdatedFromERPTime", TypeName = "datetime")]
        public DateTime? UpdatedFromErptime { get; set; }
        [StringLength(4000)]
        public string SystemNote { get; set; }
        public bool? IsNew { get; set; }
        public int GeneratedCode { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsSentToSap { get; set; }
        [StringLength(200)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string IdentityNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IdentityDate { get; set; }
        [StringLength(50)]
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
        [StringLength(100)]
        public string TaxCode { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedAccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedDateTime { get; set; }
        [StringLength(200)]
        public string SoBinhDien { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountPercent { get; set; }
        [StringLength(4000)]
        public string DiscountNote { get; set; }
        [StringLength(4000)]
        public string DeletedNote { get; set; }
        [StringLength(200)]
        public string SoThamChieu { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(CustomerModel.ServiceOrderModels))]
        public virtual CustomerModel Customer { get; set; }
        [ForeignKey(nameof(SaleOrderMasterId))]
        [InverseProperty(nameof(SaleOrderMasterModel.ServiceOrderModels))]
        public virtual SaleOrderMasterModel SaleOrderMaster { get; set; }
        [ForeignKey(nameof(VehicleId))]
        [InverseProperty(nameof(VehicleInfoModel.ServiceOrderModels))]
        public virtual VehicleInfoModel Vehicle { get; set; }
        [InverseProperty(nameof(ServiceOrderConsultModel.ServiceOrder))]
        public virtual ICollection<ServiceOrderConsultModel> ServiceOrderConsultModels { get; set; }
        [InverseProperty(nameof(ServiceOrderDetailAccessoryModel.ServiceOrder))]
        public virtual ICollection<ServiceOrderDetailAccessoryModel> ServiceOrderDetailAccessoryModels { get; set; }
        [InverseProperty(nameof(ServiceOrderDetailServiceModel.ServiceOrder))]
        public virtual ICollection<ServiceOrderDetailServiceModel> ServiceOrderDetailServiceModels { get; set; }
    }
}
