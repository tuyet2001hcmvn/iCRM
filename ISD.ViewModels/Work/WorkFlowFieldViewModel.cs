using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class WorkFlowFieldViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FieldCode")]
        public string FieldCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkFlowField_FieldName")]
        public string FieldName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsRequired")]
        public bool? IsRequired { get; set; }

        public bool? IsChoose { get; set; }

        public int? OrderIndex_Config { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Parameters")]
        public string Parameters { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkflowFieldTitle")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HideWhenAdd")]
        public Nullable<bool> HideWhenAdd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultValue")]
        public string AddDefaultValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HideWhenEdit")]
        public Nullable<bool> HideWhenEdit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultValue")]
        public string EditDefaultValue { get; set; }
    }

    public class WorkFlowFieldSearchViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FieldCode")]
        public string FieldCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkFlowField_FieldName")]
        public string FieldName { get; set; }
    }

    public class WorkFlowConfigViewModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FieldCode")]
        public string FieldCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkFlowField_FieldName")]
        public string FieldName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsRequired")]
        public bool? IsRequired { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Parameters")]
        public string Parameters { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Notes")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HideWhenAdd")]
        public Nullable<bool> HideWhenAdd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultValue")]
        public string AddDefaultValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HideWhenEdit")]
        public Nullable<bool> HideWhenEdit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultValue")]
        public string EditDefaultValue { get; set; }
    }
}
