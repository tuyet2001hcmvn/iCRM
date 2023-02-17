using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class ProvinceExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Excel_ProvinceId")]
        public System.Guid ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_ProvinceCode")]
        [Required]
        public string ProvinceCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_ProvinceName")]
        [Required]
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_Area")]
        public string Area { get; set; }

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
