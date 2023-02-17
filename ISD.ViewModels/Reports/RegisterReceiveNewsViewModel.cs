using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class RegisterReceiveNewsViewModel
    {

        public System.Guid RegisterReceiveNewsId { get; set; }
        public string CompanyCode { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        public string Companyname { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Customer_FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }
        public Nullable<System.Guid> ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public Nullable<System.Guid> DistrictId { get; set; }
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Configuration_Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Notes")]
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

    }
}