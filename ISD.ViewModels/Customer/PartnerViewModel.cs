using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class PartnerViewModel : BaseClassViewModel
    {
        public System.Guid PartnerId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerType")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]

        public string PartnerType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerType")]
        public string PartnerTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerProfileId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> PartnerProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerName")]
        public string PartnerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
    }
}
