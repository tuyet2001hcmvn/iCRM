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
    
    public partial class StoreModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StoreModel()
        {
            this.PromotionByStoreModel = new HashSet<PromotionByStoreModel>();
            this.DepartmentModel = new HashSet<DepartmentModel>();
            this.SalesEmployeeModel = new HashSet<SalesEmployeeModel>();
            this.DeliveryModel = new HashSet<DeliveryModel>();
            this.InventoryModel = new HashSet<InventoryModel>();
            this.Stock_Store_Mapping = new HashSet<Stock_Store_Mapping>();
            this.StockReceivingMasterModel = new HashSet<StockReceivingMasterModel>();
            this.StockTransferRequestModel = new HashSet<StockTransferRequestModel>();
            this.TransferModel = new HashSet<TransferModel>();
            this.MaterialMinMaxPriceModel = new HashSet<MaterialMinMaxPriceModel>();
            this.AccountModel = new HashSet<AccountModel>();
        }
    
        public System.Guid StoreId { get; set; }
        public string SaleOrgCode { get; set; }
        public Nullable<System.Guid> StoreTypeId { get; set; }
        public string StoreName { get; set; }
        public System.Guid CompanyId { get; set; }
        public string TelProduct { get; set; }
        public string TelService { get; set; }
        public string StoreAddress { get; set; }
        public string Area { get; set; }
        public Nullable<System.Guid> ProvinceId { get; set; }
        public Nullable<System.Guid> DistrictId { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string LogoUrl { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
        public string mLat { get; set; }
        public string mLong { get; set; }
        public string InvoiceStoreName { get; set; }
        public string DefaultCustomerSource { get; set; }
        public string SMSTemplateCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionByStoreModel> PromotionByStoreModel { get; set; }
        public virtual CompanyModel CompanyModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepartmentModel> DepartmentModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesEmployeeModel> SalesEmployeeModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryModel> DeliveryModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryModel> InventoryModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_Store_Mapping> Stock_Store_Mapping { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferModel> TransferModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialMinMaxPriceModel> MaterialMinMaxPriceModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountModel> AccountModel { get; set; }
    }
}
