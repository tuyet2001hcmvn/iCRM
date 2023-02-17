using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class AccessorySearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryCode", Description = "MinCode_Hint")]
        public string SearchAccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
        public string SearchAccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string SearchAccessoryCategoryId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryType")]
        //public string SearchAccessoryType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategoryType")]
        public int SearchAccessoryByCategoryType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceType")]
        public string SearchServiceTypeCode { get; set; }

        public string SaleOrg { get; set; }
    }

    public class AccessoryPromotionSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryCode", Description = "MinCode_Hint")]
        public string SearchAccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
        public string SearchAccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string SearchAccessoryCategoryId { get; set; }

        //Chương trình khuyến mãi (chỉ display những chương trình còn trong thời hạn)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategoryType")]
        public string SearchPromotionId { get; set; }

        public string SaleOrg { get; set; }
    }
}
