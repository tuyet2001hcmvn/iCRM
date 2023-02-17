using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialFreightGroupViewModel
    {
        //Mã màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialFreightGroup_MaterialFreightGroupCode")]
        public string MaterialFreightGroupCode { get; set; }
        //Tên màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialFreightGroup_MaterialFreightGroupName")]
        public string MaterialFreightGroupName { get; set; }
        //Mã RGB
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialFreightGroup_RGBCode")]
        public string RGBCode { get; set; }
        //Mã phiên bản
        public string ExternalMaterialGroupCode { get; set; }

        public string TemperatureConditionCode { get; set; }

        public string LaborMobileCode { get; set; }

        public string LaborCode { get; set; }
    }
}
