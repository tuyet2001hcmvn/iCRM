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
    
    public partial class CampaignModel
    {
        public System.Guid Id { get; set; }
        public int CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public System.Guid ContentId { get; set; }
        public System.Guid TargetGroupId { get; set; }
        public string SaleOrg { get; set; }
        public Nullable<System.Guid> Status { get; set; }
        public Nullable<bool> isImmediately { get; set; }
        public Nullable<System.DateTime> ScheduledToStart { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
        public string Type { get; set; }
    
        public virtual CatalogModel CatalogModel { get; set; }
    }
}
