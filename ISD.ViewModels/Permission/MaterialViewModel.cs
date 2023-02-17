using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MaterialViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WERKS")]
        public string WERKS { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WERKS")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ParentCategory")]
        public string ParentCategory { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category")]
        public string TPGRPNM { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MAKTX")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MAKTX { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MATNR")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        //[Remote("ValidationMaterial", "Validation", AdditionalFields = "New_MATNR, WERKS")]
        public string MATNR { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MATNR_DES")]
        public string MATNR_DES { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KOSCH")]
        public string KOSCH { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaBanLe")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? GiaBanLe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }

        public Nullable<bool> IsCreatedOnSAP { get; set; }


        //Search
        #region Search
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsFeature")]
        public Nullable<bool> IsFeature { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsNew")]
        public Nullable<bool> IsNew { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsPromo")]
        public Nullable<bool> IsPromo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsComingSoon")]
        public Nullable<bool> IsComingSoon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsChietKhau")]
        public Nullable<bool> IsChietKhau { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsCoQua")]
        public Nullable<bool> IsCoQua { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_Material3DFile")]
        public Nullable<bool> IsHas3DFile { get; set; }
        #endregion
    }
}
