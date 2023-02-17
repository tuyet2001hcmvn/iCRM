using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TemplateAndGiftCampaignSearchModel
    {
        public Guid Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignCode")]
        public int TemplateAndGiftCampaignCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignName")]
        public string TemplateAndGiftCampaignName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TemplateAndGiftTargetGroupName { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }
    }
}
