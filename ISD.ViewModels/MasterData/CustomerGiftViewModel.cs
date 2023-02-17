using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class CustomerGiftViewModel
    {
        public System.Guid GiftId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingGiftCode", "CustomerGift", AdditionalFields = "GiftCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string GiftCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string GiftName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.DateTime> EffectFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.DateTime> EffectToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Description", Description = "Promotion_Description_Hint")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ImageUrl", Description = "Promotion_ImageUrl_Hint")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Notes")]
        public string Notes { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Periodically_ApplyFor")]
        public string ApplyFor { get; set; }

        //Customer
        //public int STT { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        //public Guid CustomerId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        //public Guid? CustomerLevelId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        //public string CustomerLevelName { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        //public string CustomerCode { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerLoyaltyCard")]
        //public string CustomerLoyaltyCard { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        //public string Phone { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        //public string FullName { get; set; }
    }
}
