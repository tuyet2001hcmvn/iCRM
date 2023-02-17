using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.MasterData
{
    public class CareerViewModel
    {
        public System.Guid CareerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Career_CareerCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingCareerCode", "Career", AdditionalFields = "CareerCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string CareerCode { get; set; }

        public string CareerCodeValid { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Career_CareerName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CareerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}
