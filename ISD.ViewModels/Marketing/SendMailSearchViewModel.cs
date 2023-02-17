using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class SendMailSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignId")]
        public Guid? CampaignId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsSend")]
        public bool? IsSend { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsOpened")]
        public bool? IsOpened { get; set; }    
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsBounce")]
        public bool? IsBounce { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isConfirm")]
        public bool? isConfirm { get; set; }    
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isCheckin")]
        public bool? isCheckin { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public string Type { get; set; }
        
    }
}
