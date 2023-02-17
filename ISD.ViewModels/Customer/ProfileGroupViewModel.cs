using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileGroupViewModel : BaseClassViewModel
    {
        public System.Guid? ProfileGroupId { get; set; }
       
        public Nullable<System.Guid> ProfileId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProfileGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileGroupCode")]
        public string ProfileGroupName { get; set; }

        public string CompanyCode { get; set; }
    }
}