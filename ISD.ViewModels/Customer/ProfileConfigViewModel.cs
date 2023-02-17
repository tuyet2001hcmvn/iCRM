using ISD.EntityModels;
using ISD.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileConfigViewModel
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

        public bool? IsReadOnly { get; set; }
    }

    public class ProfileFieldViewModel
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
        
        public bool? IsReadOnly { get; set; }
    }

    public class ProfileCategoryViewModel : ProfileCategoryModel
    {
        
    }
}
