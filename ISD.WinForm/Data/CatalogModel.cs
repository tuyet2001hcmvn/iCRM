//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.WinForm.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CatalogModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CatalogModel()
        {
            this.CampaignModel = new HashSet<CampaignModel>();
        }
    
        public System.Guid CatalogId { get; set; }
        public string CatalogTypeCode { get; set; }
        public string CatalogCode { get; set; }
        public string CatalogText_en { get; set; }
        public string CatalogText_vi { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public Nullable<bool> Actived { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CampaignModel> CampaignModel { get; set; }
    }
}
