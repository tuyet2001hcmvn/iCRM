using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ColorExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ColorId")]
        public Guid ColorId { get; set; }

        //Mã màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorShortName")]
        [Required]
        public string ColorShortName { get; set; }

        //Mã RGB
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorCode")]
        [Required]
        public string ColorCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorName")]
        [Required]
        public string ColorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]

        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
