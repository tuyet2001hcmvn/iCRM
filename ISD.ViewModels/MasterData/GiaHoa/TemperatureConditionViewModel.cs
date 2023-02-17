using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class TemperatureConditionViewModel
    {
        //Mã kiểu xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TemperatureCondition_TemperatureConditionCode")]
        public string TemperatureConditionCode { get; set; }
        //Tên kiểu xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TemperatureCondition_TemperatureConditionName")]
        public string TemperatureConditionName { get; set; }
    }
}
