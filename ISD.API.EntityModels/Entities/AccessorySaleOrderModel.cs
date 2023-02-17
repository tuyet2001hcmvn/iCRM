using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessorySaleOrderModel", Schema = "ghSale")]
    public partial class AccessorySaleOrderModel
    {
        public AccessorySaleOrderModel()
        {
            AccessorySaleOrderDetailModels = new HashSet<AccessorySaleOrderDetailModel>();
        }

        [Key]
        public Guid AccessorySaleOrderId { get; set; }
        [StringLength(50)]
        public string AccessorySaleOrderCode { get; set; }
        public Guid? AccessorySellTypeId { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [StringLength(50)]
        public string ConsultEmployeeCode { get; set; }
        public Guid? CustomerId { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        [StringLength(50)]
        public string CustomerName { get; set; }
        public int? CustomerType { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(100)]
        public string CustomerAddress { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public bool? Gender { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(20)]
        public string IdentityNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IdentityDate { get; set; }
        [StringLength(50)]
        public string IdentityPlace { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(50)]
        public string TaxCode { get; set; }
        public Guid? VehicleId { get; set; }
        [StringLength(200)]
        public string LicensePlate { get; set; }
        [StringLength(200)]
        public string SerialNumber { get; set; }
        [StringLength(200)]
        public string EngineNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BuyDate { get; set; }
        [StringLength(200)]
        public string ProfitCenterName { get; set; }
        [StringLength(200)]
        public string ProductHierarchyName { get; set; }
        [StringLength(200)]
        public string MaterialGroupName { get; set; }
        [StringLength(200)]
        public string LaborName { get; set; }
        [StringLength(200)]
        public string MaterialFreightGroupName { get; set; }
        [StringLength(200)]
        public string ExternalMaterialGroupName { get; set; }
        [StringLength(200)]
        public string TemperatureConditionName { get; set; }
        [StringLength(200)]
        public string ContainerRequirementName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CurrentKilometers { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalPrice { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public int GeneratedCode { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsLayHang { get; set; }
        public bool? IsXacNhan { get; set; }
        public bool? IsCanceled { get; set; }
        [StringLength(400)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TransferCreatedDate { get; set; }
        [StringLength(50)]
        public string TransferCreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CompleteCreatedDate { get; set; }
        [StringLength(50)]
        public string CompleteCreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CancelDate { get; set; }
        [StringLength(50)]
        public string CancelUser { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountPercent { get; set; }
        [StringLength(4000)]
        public string DiscountNote { get; set; }
        [StringLength(4000)]
        public string CancelNote { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(CustomerModel.AccessorySaleOrderModels))]
        public virtual CustomerModel Customer { get; set; }
        [ForeignKey(nameof(VehicleId))]
        [InverseProperty(nameof(VehicleInfoModel.AccessorySaleOrderModels))]
        public virtual VehicleInfoModel Vehicle { get; set; }
        [InverseProperty(nameof(AccessorySaleOrderDetailModel.AccessorySaleOrder))]
        public virtual ICollection<AccessorySaleOrderDetailModel> AccessorySaleOrderDetailModels { get; set; }
    }
}
