using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class WardViewModel
    {
        public Guid WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string WardCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ward_Appellation")]
        public string Appellation { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Province")]
        public Guid? ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Province")]
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_District")]
        public Guid? DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_District")]
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public int? OrderIndex { get; set; }
    }
}
