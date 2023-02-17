using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ProspectViewModel
    {
        public Guid ProspectId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProspectCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerType")]
        public int? ProspectType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Gender")]
        public Nullable<bool> Gender { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string ProspectAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Ward")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Ward")]
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
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

        //Xe đã sử dụng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UsedMaterial")]
        public string UsedMaterial { get; set; }

        //Xe đang sử dụng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UsingMaterial")]
        public string UsingMaterial { get; set; }

        //KH tiềm năng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_IsOpportunity")]
        public bool? IsOpportunity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_Subject")]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_PurchasedTime")]
        public Guid? PurchasedTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_EstimatedRevenue")]
        public decimal? EstimatedRevenue { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_SuccessPercent", Description = "Prospect_SuccessPercent_Hint")]
        public int? SuccessPercent { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Prospect_SourceId")]
        public Guid? SourceId { get; set; }
    }
}
