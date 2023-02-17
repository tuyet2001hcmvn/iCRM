//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleOrderMasterModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleOrderMasterModel()
        {
            this.SaleOrderDetailModel = new HashSet<SaleOrderDetailModel>();
            this.ServiceOrderModel = new HashSet<ServiceOrderModel>();
        }
    
        public System.Guid SaleOrderMasterId { get; set; }
        public string SaleOrderMasterCode { get; set; }
        public Nullable<int> SaleOrderType { get; set; }
        public string Plant { get; set; }
        public string SaleOrg { get; set; }
        public Nullable<System.Guid> CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string IdentityUrl { get; set; }
        public Nullable<int> CustomerType { get; set; }
        public Nullable<System.Guid> ProvinceId { get; set; }
        public Nullable<System.Guid> DistrictId { get; set; }
        public Nullable<System.Guid> WardId { get; set; }
        public string CustomerAddress { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public Nullable<System.DateTime> IdentityDate { get; set; }
        public string IdentityPlace { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Career { get; set; }
        public string TaxCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Unit { get; set; }
        public string ProfitCenter { get; set; }
        public string ProductHierarchy { get; set; }
        public string MaterialGroup { get; set; }
        public string Labor { get; set; }
        public string MaterialFreightGroup { get; set; }
        public string ExternalMaterialGroup { get; set; }
        public string TemperatureCondition { get; set; }
        public string ContainerRequirement { get; set; }
        public string SerialNumber { get; set; }
        public string EngineNumber { get; set; }
        public string Batch { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> VATPrice { get; set; }
        public Nullable<decimal> RegisterFee { get; set; }
        public Nullable<decimal> RegisterFeeTotal { get; set; }
        public Nullable<decimal> LicensePrice { get; set; }
        public Nullable<decimal> BHTNDS { get; set; }
        public Nullable<decimal> ServiceFee { get; set; }
        public Nullable<decimal> FeeTotal { get; set; }
        public string CashCode { get; set; }
        public Nullable<decimal> DownPaymentCash { get; set; }
        public string AccountCode { get; set; }
        public Nullable<decimal> DownPaymentTransfer { get; set; }
        public string Organization { get; set; }
        public string ContractNumber { get; set; }
        public Nullable<decimal> DownPayment { get; set; }
        public Nullable<decimal> BalanceDue { get; set; }
        public string EmployeeCode { get; set; }
        public string Note { get; set; }
        public int GeneratedCode { get; set; }
        public Nullable<bool> IsGiuXe { get; set; }
        public Nullable<bool> IsChayHoSo { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<bool> IsXacNhan { get; set; }
        public Nullable<bool> IsCanceled { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string WarehouseCode { get; set; }
        public Nullable<bool> IsPrinted { get; set; }
        public string PrinterName { get; set; }
        public string ReportHtml { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<System.Guid> CanceledAccountId { get; set; }
        public Nullable<System.DateTime> CanceledDateTime { get; set; }
        public Nullable<System.Guid> XacNhanAccountId { get; set; }
        public Nullable<System.DateTime> XacNhanDateTime { get; set; }
        public Nullable<System.Guid> CareerId { get; set; }
        public string InvoiceCompanyCode { get; set; }
        public string InternalComment { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercent { get; set; }
        public string DiscountNote { get; set; }
        public string IdentityUrl2 { get; set; }
        public Nullable<decimal> SalePriceOriginal { get; set; }
        public Nullable<decimal> DiscountPercentSalePrice { get; set; }
        public Nullable<decimal> DiscountAmountSalePrice { get; set; }
        public Nullable<System.Guid> ProvinceId2 { get; set; }
        public Nullable<System.Guid> DistrictId2 { get; set; }
        public Nullable<System.Guid> WardId2 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CanceledNote { get; set; }
    
        public virtual CustomerModel CustomerModel { get; set; }
        public virtual MaterialModel MaterialModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleOrderDetailModel> SaleOrderDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceOrderModel> ServiceOrderModel { get; set; }
    }
}