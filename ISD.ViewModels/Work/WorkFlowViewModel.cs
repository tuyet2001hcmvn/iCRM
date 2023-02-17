using ISD.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class WorkFlowViewModel : WorkFlowModel
    {
        public System.Guid TaskStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public string TaskStatusName { get; set; }

        public string CreateUser { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkFlow_Step")]
        public string TaskStatusCode { get; set; }

    }

    public class TypeViewModel
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
    }

    public class WorkFlowCategoryViewModel
    {
        public Guid? WorkFlowId { get; set; }
        public string WorkFlowCategoryCode { get; set; }
        public string WorkFlowCategoryName { get; set; }
    }
}
