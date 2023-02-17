using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SaleOrderMasterModel", Schema = "ghSale")]
    public partial class SaleOrderMasterModel
    {
        public SaleOrderMasterModel()
        {
            SaleOrderDetailModels = new HashSet<SaleOrderDetailModel>();
            ServiceOrderModels = new HashSet<ServiceOrderModel>();
        }

        [Key]
        public Guid SaleOrderMasterId { get; set; }
        [StringLength(50)]
        public string SaleOrderMasterCode { get; set; }
        public int? SaleOrderType { get; set; }
        [StringLength(50)]
        public string Plant { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        public Guid? CustomerId { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        [StringLength(1000)]
        public string CustomerName { get; set; }
        public bool? Gender { get; set; }
        [StringLength(500)]
        public string IdentityUrl { get; set; }
        public int? CustomerType { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(100)]
        public string CustomerAddress { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
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
        [StringLength(200)]
        public string Career { get; set; }
        [StringLength(50)]
        public string TaxCode { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [StringLength(100)]
        public string MaterialName { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        [StringLength(100)]
        public string ProfitCenter { get; set; }
        [StringLength(100)]
        public string ProductHierarchy { get; set; }
        [StringLength(100)]
        public string MaterialGroup { get; set; }
        [StringLength(100)]
        public string Labor { get; set; }
        [StringLength(100)]
        public string MaterialFreightGroup { get; set; }
        [StringLength(100)]
        public string ExternalMaterialGroup { get; set; }
        [StringLength(100)]
        public string TemperatureCondition { get; set; }
        [StringLength(100)]
        public string ContainerRequirement { get; set; }
        [StringLength(100)]
        public string SerialNumber { get; set; }
        [StringLength(100)]
        public string EngineNumber { get; set; }
        [StringLength(50)]
        public string Batch { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalePrice { get; set; }
        [Column("VATPrice", TypeName = "decimal(18, 2)")]
        public decimal? Vatprice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RegisterFee { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RegisterFeeTotal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? LicensePrice { get; set; }
        [Column("BHTNDS", TypeName = "decimal(18, 2)")]
        public decimal? Bhtnds { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ServiceFee { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? FeeTotal { get; set; }
        [StringLength(50)]
        public string CashCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DownPaymentCash { get; set; }
        [StringLength(50)]
        public string AccountCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DownPaymentTransfer { get; set; }
        [StringLength(50)]
        public string Organization { get; set; }
        [StringLength(50)]
        public string ContractNumber { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DownPayment { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BalanceDue { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(400)]
        public string Note { get; set; }
        public int GeneratedCode { get; set; }
        public bool? IsGiuXe { get; set; }
        public bool? IsChayHoSo { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsXacNhan { get; set; }
        public bool? IsCanceled { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(50)]
        public string WarehouseCode { get; set; }
        public bool? IsPrinted { get; set; }
        [StringLength(50)]
        public string PrinterName { get; set; }
        public string ReportHtml { get; set; }
        public int? Number { get; set; }
        public Guid? CanceledAccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CanceledDateTime { get; set; }
        public Guid? XacNhanAccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? XacNhanDateTime { get; set; }
        public Guid? CareerId { get; set; }
        [StringLength(50)]
        public string InvoiceCompanyCode { get; set; }
        [StringLength(100)]
        public string InternalComment { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountPercent { get; set; }
        [StringLength(4000)]
        public string DiscountNote { get; set; }
        [StringLength(500)]
        public string IdentityUrl2 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalePriceOriginal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountPercentSalePrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountAmountSalePrice { get; set; }
        public Guid? ProvinceId2 { get; set; }
        public Guid? DistrictId2 { get; set; }
        public Guid? WardId2 { get; set; }
        [StringLength(100)]
        public string CustomerAddress2 { get; set; }
        [StringLength(4000)]
        public string CanceledNote { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(CustomerModel.SaleOrderMasterModels))]
        public virtual CustomerModel Customer { get; set; }
        [ForeignKey(nameof(MaterialCode))]
        [InverseProperty(nameof(MaterialModel.SaleOrderMasterModels))]
        public virtual MaterialModel MaterialCodeNavigation { get; set; }
        [InverseProperty(nameof(SaleOrderDetailModel.SaleOrderMaster))]
        public virtual ICollection<SaleOrderDetailModel> SaleOrderDetailModels { get; set; }
        [InverseProperty(nameof(ServiceOrderModel.SaleOrderMaster))]
        public virtual ICollection<ServiceOrderModel> ServiceOrderModels { get; set; }
    }
}
