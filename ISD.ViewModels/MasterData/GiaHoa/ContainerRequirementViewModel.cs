using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ContainerRequirementViewModel
    {
        //Mã option
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContainerRequirement_ContainerRequirementCode")]
        public string ContainerRequirementCode { get; set; }
        //Tên option
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContainerRequirement_ContainerRequirementName")]
        public string ContainerRequirementName { get; set; }
    }
}
