using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ExternalMaterialGroupViewModel
    {
        //Mã đời xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExternalMaterialGroup_ExternalMaterialGroupCode")]
        public string ExternalMaterialGroupCode { get; set; }
        //Tên đời xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExternalMaterialGroup_ExternalMaterialGroupName")]
        public string ExternalMaterialGroupName { get; set; }
    }
}
