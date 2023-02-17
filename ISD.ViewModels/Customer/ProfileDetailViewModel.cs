using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileDetailViewModel : ProfileModel
    {
        public string CustomerTypeName { get; set; }

        //Bussiness
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Position")]
        public string PositionB { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
        public string CustomerCategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
        public string CustomerCareerCode { get; set; }

        //Customer
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyId")]
        public Nullable<System.Guid> CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Position")]
        public string PositionC { get; set; }
        public string CompanyName { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string CustomerCategoryName { get; set; }
        public string CustomerCareerName { get; set; }
    }
}
