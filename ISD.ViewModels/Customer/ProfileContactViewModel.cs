using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ProfileContactViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
        public string Email { get; set; }

        public Guid ProfileContactId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileName")]
        public string ProfileContactName { get; set; }
        public string ProfileContactFullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        [RegularExpression("^(([+]{1}[0-9]{2}|0)[0-9]{9})$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Phone")]
        [Remote("CheckExistingProfileContactPhone", "Profile", AdditionalFields = "ProfileContactPhoneValid, ProfileContactTypeCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string ProfileContactPhone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Email")]
        [Remote("CheckExistingProfileContactEmail", "Profile", AdditionalFields = "ProfileContactEmailValid, ProfileContactTypeCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string ProfileContactEmail { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string ProfileContactAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public Nullable<System.Guid> ProfileContactProvinceId { get; set; }
        public string ProfileContactProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public Nullable<System.Guid> ProfileContactDistrictId { get; set; }
        public string ProfileContactDistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public Nullable<System.Guid> ProfileContactCompanyId { get; set; }
        public string ProfileContactCompanyName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Position")]
        public string ProfileContactPosition { get; set; }

        public string ProfileContactWardName { get; set; }
        public string ProfileContactShortName { get; set; }
        public string ProfileContactDepartment { get; set; }
        public string ProfileContactPositionName { get; set; }
        public string ProfileContactDepartmentName { get; set; }

        public bool? IsMain { get; set; }
    }
}
