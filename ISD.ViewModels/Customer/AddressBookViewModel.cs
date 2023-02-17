using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AddressBookViewModel : BaseClassViewModel
    {
        public System.Guid AddressBookId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_AddressTypeCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string AddressTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Address")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Address2")]
        public string Address2 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public Nullable<System.Guid> DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public Nullable<System.Guid> WardId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Country")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsForeignCustomer")]
        public string CountryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Note")]
        public string Note { get; set; }
        public string AddressTypeName { get; set; }
        public string CountryName { get; set; }
        //Address
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string SaleOfficeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_isMain")]
        public Nullable<bool> isMain { get; set; }
        public Nullable<bool> CheckAddress { get; set; }
    }
}
