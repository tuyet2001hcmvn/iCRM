using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class PlateFeeViewModel
    {
        public System.Guid PlateFeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingPlateFeeCode", "PlateFee", AdditionalFields = "PlateFeeCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string PlateFeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PlateFeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Periodically_ApplyFor")]
        public string ApplyFor { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFee_DetailPlateFee")]
        public string DetailPlateFee { get; set; }

        //public List<Detail_PeriodicallyCheckingViewModel> chosenList { get; set; }
    }
}
