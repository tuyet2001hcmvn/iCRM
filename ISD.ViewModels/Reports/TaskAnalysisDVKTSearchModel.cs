using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskAnalysisDVKTSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public string ServiceTechnicalTeamCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public Guid? DepartmentId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public Guid? WorkFlowId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string TaskStatusCode { get; set; }
        [Display( Name = "Thời gian lọc")]
        public string CommonDate { get; set; }
      
        public DateTime? CurrentFromDate { get; set; }
      
        public DateTime? CurrentToDate { get; set; }
        public DateTime? PreviousFromDate { get; set; }

        public DateTime? PreviousToDate { get; set; }
        public bool IsView { get; set; }
    }
}
