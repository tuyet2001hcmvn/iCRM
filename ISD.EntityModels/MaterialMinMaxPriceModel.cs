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
    
    public partial class MaterialMinMaxPriceModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaterialMinMaxPriceModel()
        {
            this.StoreModel = new HashSet<StoreModel>();
        }
    
        public System.Guid MaterialMinMaxPriceId { get; set; }
        public string MaterialCode { get; set; }
        public Nullable<decimal> MinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public Nullable<System.DateTime> EffectedFromDate { get; set; }
        public Nullable<System.DateTime> EffectedToDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string LastModifiedUser { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
    
        public virtual MaterialModel MaterialModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreModel> StoreModel { get; set; }
    }
}
