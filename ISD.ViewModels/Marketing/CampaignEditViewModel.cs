using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Marketing
{
    public class CampaignEditViewModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignCode")]
        public int CampaignCode { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignName")]
        public string CampaignName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }
        public Guid ContentId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentName")]
        public string ContentName { get; set; }
        public Guid TargetGroupId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TargetGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrg")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string StatusName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Immediately")]
        public bool IsImmediately { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScheduledToStart")]
        public DateTime ScheduledToStart { get; set; }

        public string Type { get; set; }
    }
}
