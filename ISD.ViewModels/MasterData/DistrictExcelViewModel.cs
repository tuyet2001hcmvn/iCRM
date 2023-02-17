using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class DistrictExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Excel_DistrictId")]
        public System.Guid DistrictId { get; set; }
        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "District_DistrictCode")]
        [Required]
        public string DistrictCode { get; set; }

        public Guid ProvinceId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Province")]
        [Required]
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "District_Appellation")]
        [Required]
        public string Appellation { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "District_DistrictName")]
        [Required]
        public string DistrictName { get; set; }

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
