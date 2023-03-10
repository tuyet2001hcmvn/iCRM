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
    
    public partial class PlateFeeModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlateFeeModel()
        {
            this.PlateFeeDetailModel = new HashSet<PlateFeeDetailModel>();
            this.ProductModel = new HashSet<ProductModel>();
        }
    
        public System.Guid PlateFeeId { get; set; }
        public string PlateFeeCode { get; set; }
        public string PlateFeeName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Actived { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> LastModifyDate { get; set; }
        public string LastModifyUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlateFeeDetailModel> PlateFeeDetailModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductModel> ProductModel { get; set; }
    }
}
