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
    
    public partial class StockTransferRequestDetailModel
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> StockTransferRequestId { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public Nullable<decimal> RequestQuantity { get; set; }
        public Nullable<decimal> TransferredQuantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> OfferQuantity { get; set; }
    
        public virtual ProductModel ProductModel { get; set; }
        public virtual StockTransferRequestModel StockTransferRequestModel { get; set; }
    }
}