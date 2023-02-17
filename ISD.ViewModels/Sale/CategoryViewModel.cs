using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels.Sale
{
    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_CategoryCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingCategoryCode", "Category", AdditionalFields = "CategoryCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_CategoryName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
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

        public bool isDelete { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_TrackTrend")]
        public bool? IsTrackTrend { get; set; }
    }
}
