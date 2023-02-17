using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class BrandExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BrandId")]
        public Guid CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Brand_BrandCode")]
        [Required]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Brand_BrandName")]
        [Required]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public Guid? ParentCategoryId { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductTypeId")]
        public int? ProductTypeId { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public string ParentCategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_ImageUrl", Description = "Category_ImageUrl_Hint")]
        public string ImageUrl { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
