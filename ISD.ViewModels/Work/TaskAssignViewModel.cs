using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskAssignViewModel
    {
        public System.Guid? TaskAssignId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskAssign_SalesEmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_RoleCode")]
        public string TaskAssignTypeCode { get; set; }
        public string TaskAssignTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Group")]
        public string RolesCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Group")]
        public string RolesName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        //Mobile additional field
        public string SalesEmployeeName { get; set; }
        public string LogoName { get; set; }

        public string RoleName { get; set; }

    }
}