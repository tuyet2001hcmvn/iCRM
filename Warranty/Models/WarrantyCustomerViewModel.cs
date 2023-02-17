using ISD.Resources;
using ISD.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Warranty.Models
{
    public class WarrantyCustomerViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_CustomerTypeCode", Description = "Profile_CustomerTypeCode_Tooltip")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CustomerTypeCode { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Profile_Title")]
        // public string Title { get; set; }

        public string CustomerTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ProfileName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ProfileShortName")]
        public string ProfileShortName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Phone")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Validation_Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Age")]
        public string Age { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ProvinceId")]
        public Nullable<System.Guid> ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_DistrictId")]
        public Nullable<System.Guid> DistrictId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WardId")]
        public Nullable<System.Guid> WardId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_CompanyNumber")]
       // [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CompanyNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Department")]
        public string DepartmentCode { get; set; }

        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ProvinceId")]
        // [RequiredIf("Type", "Account", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? RequiredProvinceId
        {
            get
            {
                return ProvinceId;
            }
            set
            {
                ProvinceId = value;
            }
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Position")]
        public string PositionB { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Phone")]
        public string PhoneBusiness { get; set; }

        public string EmailBusiness { get; set; }
    }
}