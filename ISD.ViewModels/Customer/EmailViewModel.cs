using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class EmailViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email_ToEmail")]
        public string ToEmail { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email_Subject")]
        public string Subject { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email_Content")]
        public string EmailContent { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email_Attachment")]
        [Required]
        public string Attachment { get; set; }
        public ProfileSearchViewModel SearchProfileData { get; set; }

        public string Cc { get; set; }
    }
}
