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
    
    public partial class PaymentNationalBudgetModel
    {
        public System.Guid PaymentNationalId { get; set; }
        public string PaymentNationalCode { get; set; }
        public string BankName { get; set; }
        public string Account { get; set; }
        public Nullable<bool> Actived { get; set; }
    }
}