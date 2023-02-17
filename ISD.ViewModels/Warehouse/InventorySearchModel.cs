using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class InventorySearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string ProductCodeSearch { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public Guid? SearchProductId { get; set; }
    }
}
