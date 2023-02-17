using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warranty.Models
{
    public class RequestWarrantyViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ContactName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_ContactNumber")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Request")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Summary { get; set; }
        //public string SeriNo { get; set; }
        //public Guid ProductWarrantyId { get; set; }
        public Guid ProfileId { get; set; }
    }
}