using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TemplateAndGiftCampaignEditModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignCode")]
        public int TemplateAndGiftCampaignCode { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignName")]
        public string TemplateAndGiftCampaignName { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TemplateAndGiftTargetGroupName { get; set; }
        [Required]
        public string TemplateAndGiftTargetGroupId { get; set; }
    }
}
