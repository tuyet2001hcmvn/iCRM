using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Service
{
    public class AccessorySellTypeViewModel
    {
        public Guid AccessorySellTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessorySellTypeCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingAccessorySellTypeCode", "AccessorySellType", AdditionalFields = "AccessorySellTypeCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string AccessorySellTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessorySellTypeName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string AccessorySellTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsTinhTien")]
        public bool? IsTinhTien { get; set; }

        //Flag dịch vụ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceFlag")]
        public Guid? ServiceFlagId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceFlag")]
        public string ServiceFlagCode { get; set; }
    }
}
