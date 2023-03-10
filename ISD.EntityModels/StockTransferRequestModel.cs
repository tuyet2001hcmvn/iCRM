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
    
    public partial class StockTransferRequestModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransferRequestModel()
        {
            this.StockTransferRequestDetailModel = new HashSet<StockTransferRequestDetailModel>();
        }
    
        public System.Guid Id { get; set; }
        public int StockTransferRequestCode { get; set; }
        public Nullable<System.Guid> FromStock { get; set; }
        public Nullable<System.Guid> ToStock { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }
        public Nullable<System.Guid> StoreId { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> Actived { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
        public string SalesEmployeeCode { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public Nullable<System.Guid> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCompany { get; set; }
        public string SenderAddress { get; set; }
        public string DeletedReason { get; set; }
        public Nullable<System.DateTime> FromPlanDate { get; set; }
        public Nullable<System.DateTime> ToPlanDate { get; set; }
    
        public virtual AccountModel AccountModel { get; set; }
        public virtual AccountModel AccountModel1 { get; set; }
        public virtual AccountModel AccountModel2 { get; set; }
        public virtual CompanyModel CompanyModel { get; set; }
        public virtual StoreModel StoreModel { get; set; }
        public virtual StockModel StockModel { get; set; }
        public virtual StockModel StockModel1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferRequestDetailModel> StockTransferRequestDetailModel { get; set; }
    }
}
