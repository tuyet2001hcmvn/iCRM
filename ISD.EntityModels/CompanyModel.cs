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
    
    public partial class CompanyModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyModel()
        {
            this.DeliveryModel = new HashSet<DeliveryModel>();
            this.InventoryModel = new HashSet<InventoryModel>();
            this.ProductModel = new HashSet<ProductModel>();
            this.SalesEmployeeModel = new HashSet<SalesEmployeeModel>();
            this.StockReceivingMasterModel = new HashSet<StockReceivingMasterModel>();
            this.StockTransferRequestModel = new HashSet<StockTransferRequestModel>();
            this.StoreModel = new HashSet<StoreModel>();
            this.TransferModel = new HashSet<TransferModel>();
        }
    
        public System.Guid CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string CompanyName { get; set; }
        public string TelProduct { get; set; }
        public string TelService { get; set; }
        public string CompanyAddress { get; set; }
        public string Logo { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
        public string Fax { get; set; }
        public string TaxCode { get; set; }
        public string CompanyShortName { get; set; }
        public string SMSTemplateCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryModel> DeliveryModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryModel> InventoryModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductModel> ProductModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesEmployeeModel> SalesEmployeeModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreModel> StoreModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferModel> TransferModel { get; set; }
    }
}
