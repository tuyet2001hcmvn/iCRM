using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ResourceViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ResourceKey")]
        public string ResourceKey { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ResourceValue")]
        public string ResourceValue { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ResourceComment")]
        public string ResourceComment { get; set; }
    }
}
