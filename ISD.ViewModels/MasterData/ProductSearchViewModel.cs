using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.MasterData
{
    public class ProductSearchViewModelRemove
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductName")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Category")]
        public Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}