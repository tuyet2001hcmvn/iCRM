using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialPropertiesViewModel
    {
        public System.Guid PropertiesId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Subject")]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }
        //Hình ảnh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string ImageUrl { get; set; }
    }
}
