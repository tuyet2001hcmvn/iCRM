using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ConfigurationViewModel
    {
        //Tab 1
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_Logo", Description = "Configuration_Logo_Hint")]
        public string Logo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_Icon", Description = "Configuration_Icon_Hint")]
        public string Icon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_isAllowedToBooking")]
        public bool? isAllowedToBooking { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_isRequiredLogin")]
        public bool? isRequiredLogin { get; set; }

        //Tab 2
        public string Token { get; set; }

        public string Key { get; set; }

        //Tab 3
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_AboutTitle")]
        public string AboutTitle { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_AboutDescription")]
        public string AboutDescription { get; set; }

        //Tab 4
        public Guid ContactId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ReviewDescription")]
        public string ReviewDescription { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ContactDescription")]
        public string ContactDescription { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ContactDetail")]
        public string ContactDetail { get; set; }

        //Tab 5
        public System.Guid BannerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigProspect_ImageUrl")]
        public string ImageUrl { get; set; }

        //Tab 6
        public string Email { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public Nullable<bool> EnableSsl { get; set; }
        public string SmtpMailFrom { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigEmail_ToEmail")]
        public string ToEmail { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigProspect_CCMail")]
        public string CCMail { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigProspect_BCCMail")]
        public string BCCMail { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigProspect_EmailTitle")]
        public string EmailTitle { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigProspect_EmailContent")]
        public string EmailContent { get; set; }

        //Tab 7
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isReceiveInCurrentDay")]
        public bool? isReceiveInCurrentDay { get; set; }
    }
}
