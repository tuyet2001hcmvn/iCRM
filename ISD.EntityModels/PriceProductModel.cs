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
    
    public partial class PriceProductModel
    {
        public System.Guid PriceProductId { get; set; }
        public System.Guid ProductId { get; set; }
        public Nullable<System.Guid> StyleId { get; set; }
        public Nullable<System.Guid> MainColorId { get; set; }
        public string PriceProductCode { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<System.TimeSpan> PostTime { get; set; }
        public string UserPost { get; set; }
    
        public virtual ProductModel ProductModel { get; set; }
        public virtual StyleModel StyleModel { get; set; }
    }
}
