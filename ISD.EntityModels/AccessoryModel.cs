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
    
    public partial class AccessoryModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccessoryModel()
        {
            this.AccessoryDetailModel = new HashSet<AccessoryDetailModel>();
            this.AccessoryProductModel = new HashSet<AccessoryProductModel>();
        }
    
        public System.Guid AccessoryId { get; set; }
        public System.Guid AccessoryCategoryId { get; set; }
        public string AccessoryCode { get; set; }
        public string AccessoryName { get; set; }
        public Nullable<bool> isHelmet { get; set; }
        public Nullable<bool> isHelmetAdult { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessoryDetailModel> AccessoryDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessoryProductModel> AccessoryProductModel { get; set; }
    }
}