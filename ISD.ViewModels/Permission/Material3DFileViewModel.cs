using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class Material3DFileViewModel
    {
        public int UploadFileId { get; set; }

        public string WERKS { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MATNR")]
        public string MATNR { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MATNR")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ObjFile")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ObjFile { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MtlFile")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MtlFile { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageFile")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ImageFile1 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageFile")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ImageFile2 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageFile")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ImageFile3 { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Number")]
        public Nullable<decimal> Width { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Number")]
        public Nullable<decimal> Height { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Number")]
        public Nullable<decimal> Depth { get; set; }
        public Nullable<decimal> Scale { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WERKS")]
        public string CompanyName { get; set; }

        public bool isHasValue { get; set; }
    }
}
