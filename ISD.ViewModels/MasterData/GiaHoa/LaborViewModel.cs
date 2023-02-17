using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class LaborViewModel
    {
        //Mã phiên bản
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Labor_LaborCode")]
        public string LaborCode { get; set; }
        //Tên phiên bản
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Labor_LaborName")]
        public string LaborName { get; set; }

        public string ExternalMaterialGroupCode { get; set; }
        public string TemperatureConditionCode { get; set; }
        public string LaborMobileCode { get; set; }
    }
}
