using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ConfigurationExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfigurationId")]
        public System.Guid ConfigurationId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationCode")]
        [Required]
        public string ConfigurationCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationName")]
        [Required]
        public string ConfigurationName { get; set; }

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
