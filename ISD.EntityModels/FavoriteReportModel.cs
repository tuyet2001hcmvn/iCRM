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
    
    public partial class FavoriteReportModel
    {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public System.Guid PageId { get; set; }
    
        public virtual AccountModel AccountModel { get; set; }
        public virtual PageModel PageModel { get; set; }
    }
}