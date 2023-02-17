using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceCustomerViewModel
    {
        public Guid CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerLoyaltyCard")]
        public string CustomerLoyaltyCard { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        public Guid? CustomerLevelId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        public string CustomerLevelName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_MiddleName")]
        public string MiddleName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Gender")]
        public Nullable<bool> Gender { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public Guid? DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public Guid? ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_EmailAddress")]
        public string EmailAddress { get; set; }

        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CumulativePoint")]
        public decimal? CumulativePoint { get; set; }

        // CustomerType
        public int? CustomerType { get; set; }
        // WardId
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public Nullable<System.Guid> WardId { get; set; }
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }
    }
}
