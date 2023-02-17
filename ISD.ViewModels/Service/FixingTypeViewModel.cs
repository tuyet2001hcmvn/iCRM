using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Service
{
    public class FixingTypeViewModel
    {
        public Guid FixingTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FixingTypeCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingFixingTypeCode", "FixingType", AdditionalFields = "FixingTypeCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string FixingTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FixingTypeName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FixingTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsTinhTien")]
        public bool? IsTinhTien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsTinhTien")]
        public bool? IsBaoHanh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsKhieuNai")]
        public bool IsKhieuNai { get; set; }

        //Flag dịch vụ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceFlag")]
        public Guid? ServiceFlagId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceFlag")]
        public string ServiceFlagCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategoryType")]
        public bool? IsAccessory { get; set; }
    }
}
