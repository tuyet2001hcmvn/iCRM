using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class Search_PeriodicallyCheckingViewModel
    {
        public System.Guid PeriodicallyCheckingId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PeriodicallyCheckingCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingPeriodicallyCheckingCode", "PeriodicallyChecking", AdditionalFields = "PeriodicallyCheckingCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string PeriodicallyCheckingCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PeriodicallyCheckingName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PeriodicallyCheckingName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Image")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberOfProduct")]
        public int? NumberOfProduct { get; set; }
    }
}
