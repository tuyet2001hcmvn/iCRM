using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AutoConditionViewModel
    {
        public System.Guid AutoConditionId { get; set; }
        public Nullable<System.Guid> StatusTransitionId { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "AdditionalSQLText")]
        public string AdditionalSQLText { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ConditionType")]
        public string ConditionType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Field")]
        public string Field { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ComparisonType")]
        public string ComparisonType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ValueType")]
        public string ValueType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Value")]
        public string Value { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_SQLText")]
        public string SQLText { get; set; }
    }
}
