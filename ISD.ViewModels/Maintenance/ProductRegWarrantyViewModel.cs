using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductRegWarrantyViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarranty_SerriNo")]
        public string Serial { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Material_MaterialModel")]
        public string ProductName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Product_ProductCode")]
        public string ProductCode { get; set; }

        public string ProfileForeignCode { get; set; }
        public string Duration { get; set; }

        public string SaleOrder { get; set; }
        public decimal? ActivatedQuantity { get; set; }

        public DateTime? DocumentDate { get; set; }
        public DateTime? PostDate { get; set; }
    }
}
