using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskStatusViewModel
    {
        public Guid? TaskStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatusCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TaskStatusCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TaskStatusName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus_ProcessCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_WorkFlowId")]
        public Nullable<System.Guid> WorkFlowId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
        public string StatusTransition { get; set; }
        public Guid? StatusTransitionId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category")]
        public string Category { get; set; }
        public string color { set; get; }

        public int? PositionLef { set; get; }
        public int? PositionRight { set; get; }
        public string typeShape { set; get; }
        public string taskId { set; get; }
        public string BranchName { get; set; }
        public Nullable<int> BranchPositionLeft { get; set; }
        public Nullable<int> BranchPositionRight { get; set; }
        public string BranchIn { get; set; }
        public string BranchOut { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AutoUpdateEndDate")]
        public bool? AutoUpdateEndDate { get; set; }

    }
}