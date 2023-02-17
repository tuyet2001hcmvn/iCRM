using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
        public Guid CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerType")]
        public int? CustomerType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DateOfIssue")]
        public DateTime? IdentityDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlaceOfIssue")]
        public string IdentityPlace { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Gender")]
        public Nullable<bool> Gender { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Ward")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Ward")]
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_District")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_District")]
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Province")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Province")]
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone2")]
        public string Phone2 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_EmailAddress")]
        public string EmailAddress { get; set; }

        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IdentityUrl")]
        public string IdentityUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CreatedUser")]
        public string CreatedUser { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IdentityUrl2")]
        public string IdentityUrl2 { get; set; }
        
        public string IdentityUrlDisplay { get; set; }
        public string IdentityUrl2Display { get; set; }

        //Chiết khấu 
        [Display(Name = "Chiết khấu")]
        public decimal? Discount { get; set; }
    }
}
