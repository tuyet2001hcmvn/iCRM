using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Customer
{
    public class ProfileBSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileName")]
        public string ProfileNameSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNoSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        public string ProfilePhoneSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
        public string ProfileEmailSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public Guid? ProvinceIdSearchList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public Guid? DistrictIdSearchList { get; set; }

    }
}
