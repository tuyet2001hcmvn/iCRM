using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialSpecificationsViewModel
    {
        public System.Guid MaterialSpecificationsId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Specifications")]
        public Guid? SpecificationsId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Specifications")]
        public string SpecificationsName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Specifications_Description { get; set; }
    }
}
