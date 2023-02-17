using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialAccessoryViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_AccessoryCategory")]
        public string AccessoryCategoryName { get; set; }

        public Guid? AccessoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Accessory")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string UOM { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_UnitPrice")]
        public decimal? UnitPrice { get; set; }
    }
}
