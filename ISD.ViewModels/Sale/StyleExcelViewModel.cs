using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class StyleExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StyleId")]
        public System.Guid StyleId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Style_StyleCode")]
        [Required]
        public string StyleCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Style_StyleName")]
        [Required]
        public string StyleName { get; set; }

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
