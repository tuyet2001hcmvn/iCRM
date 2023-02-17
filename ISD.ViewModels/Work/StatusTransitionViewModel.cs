using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StatusTransitionViewModel
    {
        public System.Guid StatusTransitionId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransitionName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TransitionName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_WorkFlowId")]
        public Nullable<System.Guid> WorkFlowId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DestinationStatusId")]
        public Nullable<System.Guid> FromStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransitionStatusId")]
        public Nullable<System.Guid> ToStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isAssigneePermission")]
        public Nullable<bool> isAssigneePermission { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isReporterPermission")]
        public Nullable<bool> isReporterPermission { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ColorName")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isCreateUserPermission")]
        public Nullable<bool> isCreateUserPermission { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isAutomaticTransitions")]
        public Nullable<bool> isAutomaticTransitions { get; set; }

        public string StatusTransitionIn { set; get; }

        public string StatusTransitionOut { set; get; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isRequiredComment")]
        public Nullable<bool> isRequiredComment { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BranchName")]
        public string BranchName { get; set; }
        public string unsignedBranchName { get; set; }
    }
}
