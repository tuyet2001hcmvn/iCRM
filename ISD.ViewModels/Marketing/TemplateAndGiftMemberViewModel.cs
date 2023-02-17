using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TemplateAndGiftMemberViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public string ProfileCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public string ProfileForeignCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileName")]
        public string ProfileName { get; set; }
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }
    }
}
