using System;

namespace ISD.API.ViewModels.MarketingViewModels.CampaignViewModels
{
    public class CampaignViewViewModel
    {
        public Guid Id { get; set; }
        public int CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Guid ContentId { get; set; }
        public string ContentName { get; set; }
        public Guid TargetGroupId { get; set; }
        public string TargetGroupName { get; set; }
        public string SaleOrg { get; set; }
        public string StatusName { get; set; }
        public bool? IsImmediately { get; set; }
        public DateTime? ScheduledToStart { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
