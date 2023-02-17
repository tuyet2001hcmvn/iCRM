using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Marketing
{
    public class MemberOfTargetGroupViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public string ProfileCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public string ProfileForeignCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfilePhone")]
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
