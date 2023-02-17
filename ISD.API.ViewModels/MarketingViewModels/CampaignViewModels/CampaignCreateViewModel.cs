using ISD.API.ViewModels.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.API.ViewModels.MarketingViewModels.CampaignViewModels
{
    public class CampaignCreateViewModel
    {
        [Required]       
        public string CampaignName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [NotEmpty]
        public Guid ContentId { get; set; }
        [Required]
        [NotEmpty]
        public Guid TargetGroupId { get; set; }
        public string ContentName { get; set; }      
        public string TargetGroupName { get; set; }
        public Guid CreateBy { get; set; }     
        public string Description { get; set; }      
        public string SaleOrg { get; set; }
       
        public bool IsImmediately { get; set; }
        public DateTime? ScheduledToStart { get; set; }
    }
}
