using System;

namespace ISD.API.EntityModels.Entities.PartialEntityModels
{
    public partial class CampaignModel
    {
        public Guid Id { get; set; }
        public int CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public Guid ContentId { get; set; }
        public Guid TargetGroupId { get; set; }
        public string SaleOrg { get; set; }
        public Guid? Status { get; set; }
        public bool? IsImmediately { get; set; }
        public DateTime? ScheduledToStart { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public virtual ContentModel Content { get; set; }
        public virtual TargetGroupModel TargetGroup { get; set; }
        public virtual AccountModel CreateByNavigation { get; set; }
        public virtual AccountModel LastEditByNavigation { get; set; }
        public virtual CatalogModel StatusNavigation { get; set; }
    }
}
