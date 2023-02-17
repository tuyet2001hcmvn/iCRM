using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Service
{
    public class ServiceFlagViewModel
    {
        public Guid ServiceFlagId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceFlagCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingServiceFlagCode", "ServiceFlag", AdditionalFields = "ServiceFlagCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string ServiceFlagCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsTinhTien")]
        public bool? IsTinhTien { get; set; }
    }
}
