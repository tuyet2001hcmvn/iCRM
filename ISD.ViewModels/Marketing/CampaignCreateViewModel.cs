using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Marketing
{
    public class CampaignCreateViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignName")]
        public string CampaignName { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SelectContentName")]
        public string ContentName { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SelectTargetGroupName")]
        public string TargetGroupName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SelectContentName")]
        [Required]
        public string ContentId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SelectTargetGroupName")]
        [Required]
        public string TargetGroupId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }
        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrg")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string Status { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Immediately")]
        public bool IsImmediately { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScheduledToStart")]
        public DateTime ScheduledToStart { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
