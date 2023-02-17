using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class PromotionViewModel
    {
        public System.Guid PromotionId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingPromotionCode", "Promotion", AdditionalFields = "PromotionCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PromotionName { get; set; }

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

        //Product
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public Guid ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ERPProductCode")]
        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public string ProductName { get; set; }
    }
    public class PromotionAPIViewModel
    {
        public string PromotionCode { get; set; }

        public string PromotionName { get; set; }

        public string Description { get; set; }
    }
}
