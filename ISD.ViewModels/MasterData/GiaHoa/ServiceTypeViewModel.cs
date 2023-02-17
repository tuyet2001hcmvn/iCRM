using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ServiceTypeViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceTypeCode")]
        public string ServiceTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceTypeName")]
        public string ServiceTypeName { get; set; }
    }
}
