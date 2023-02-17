using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class Search_PlateFeeViewModel
    {
        public Guid PlateFeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PlateFeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeName")]
        public string PlateFeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberOfProduct")]
        public int? NumberOfProduct { get; set; }
    }
}
