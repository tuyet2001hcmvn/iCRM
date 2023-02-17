using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class AccessoryViewModel
    {
        public System.Guid AccessoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public System.Guid AccessoryCategoryId { get; set; }

        public string AccessoryCategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingAccessoryCode", "Accessory", AdditionalFields = "AccessoryCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_ImageUrl")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        //Image Detail
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_isHelmet")]
        public Nullable<bool> isHelmet { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_isHelmetAdult")]
        public Nullable<bool> isHelmetAdult { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Size")]
        public string Size { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_HelmetColorId")]
        public Nullable<System.Guid> HelmetColorId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_HelmetColorId")]
        public string HelmetColorCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_HelmetColorId")]
        public string HelmetColorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_NumberOfImage")]
        public int NumberOfImage { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_HelmetType")]
        public string HelmetType { get; set; }
    }
}
