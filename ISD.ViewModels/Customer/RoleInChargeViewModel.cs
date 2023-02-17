using ISD.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class RoleInChargeViewModel : BaseClassViewModel
    {
        public System.Guid RoleInChargeId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RoleInCharge_RoleCode")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]

        public Nullable<System.Guid> RolesId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RoleInCharge_RoleCode")]
        public string RoleCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RoleInCharge")]
        public string RoleName { get; set; }
    }
}