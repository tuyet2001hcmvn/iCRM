using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class CategoryExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CategoryId")]
        public Guid CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_CategoryCode")]
        [Required]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_CategoryName")]
        [Required]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        [Required]
        public string BrandName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        public Guid? ParentCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductTypeId")]
        public int? ProductTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductTypeId")]
        public string ProductTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_TrackTrend")]
        public bool? IsTrackTrend { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
