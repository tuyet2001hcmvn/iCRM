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
    
    public partial class AddressBookModel
    {
        public System.Guid AddressBookId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }
        public string AddressTypeCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public Nullable<System.Guid> ProvinceId { get; set; }
        public Nullable<System.Guid> DistrictId { get; set; }
        public Nullable<System.Guid> WardId { get; set; }
        public string CountryCode { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
        public Nullable<bool> isMain { get; set; }
    
        public virtual ProfileModel ProfileModel { get; set; }
    }
}