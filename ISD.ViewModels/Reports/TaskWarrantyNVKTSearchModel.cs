using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskWarrantyNVKTSearchModel
    {
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }
        public string EndCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public string TaskStatusCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskProcessCode")]
        public string TaskProcessCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public List<string> ServiceTechnicalTeamCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Assignee")]
        public string Assignee { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<Guid> DepartmentId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public List<Guid> WorkFlowId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public Nullable<System.Guid> ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public Nullable<System.Guid> ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerResult")]
        public string Property5 { get; set; }
        public bool IsView { get; set; }
    }
}
