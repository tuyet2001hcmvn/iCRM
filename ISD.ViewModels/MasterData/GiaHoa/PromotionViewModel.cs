using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Sale
{
    public class PromotionViewModel
    {
        public Guid PromotionId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingPromotionCode", "Promotion", AdditionalFields = "PromotionCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PromotionName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionContent")]
        public string PromotionContent { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public Guid? CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
        public DateTime? ToDate { get; set; }

        //Mapping Store
        public List<StoreModel> ActivedStoreList { get; set; }

        public Guid? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedUser { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
