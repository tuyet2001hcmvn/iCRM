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
    
    public partial class MaterialPriceModel
    {
        public System.Guid MaterialPriceId { get; set; }
        public string MaterialCode { get; set; }
        public Nullable<decimal> InvoicePrice { get; set; }
        public Nullable<decimal> RegistrationFeePrice { get; set; }
        public Nullable<System.Guid> DistrictId { get; set; }
        public Nullable<System.DateTime> EffectedFromDate { get; set; }
        public Nullable<System.DateTime> EffectedToDate { get; set; }
    
        public virtual MaterialModel MaterialModel { get; set; }
    }
}