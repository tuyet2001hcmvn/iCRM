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
    
    public partial class DeliveryDetailModel
    {
        public System.Guid DeliveryDetailId { get; set; }
        public Nullable<System.Guid> DeliveryId { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public Nullable<System.Guid> StockId { get; set; }
        public Nullable<System.Guid> InventoryId { get; set; }
        public Nullable<int> DateKey { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual ProductModel ProductModel { get; set; }
        public virtual DeliveryModel DeliveryModel { get; set; }
        public virtual DimDateModel DimDateModel { get; set; }
        public virtual InventoryModel InventoryModel { get; set; }
        public virtual StockModel StockModel { get; set; }
    }
}
