using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISD.EntityModels;

namespace ISD.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewPassword")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NewPassword { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "retypeNewPassword")]
        [NotMapped]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [CompareAttribute("NewPassword", ErrorMessage = "", 
                                      ErrorMessageResourceName = "CheckretypePassword", 
                                      ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string retypeNewPassword { get; set; }

    }
}
