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
    
    public partial class CategoryModel
    {
        public System.Guid CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<System.Guid> ParentCategoryId { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }
        public Nullable<bool> IsTrackTrend { get; set; }
    }
}
