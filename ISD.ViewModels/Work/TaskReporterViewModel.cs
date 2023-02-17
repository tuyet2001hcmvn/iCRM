using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskReporterViewModel
    {
        public System.Guid? TaskReporterId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskAssign_SalesEmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        //Mobile additional field
        public string SalesEmployeeName { get; set; }
        public string Reporter { get; set; }
        public string ReporterName { get; set; }
        public string ReporterLogoName { get; set; }
        public string TaskAssignTypeCode { get; set; }
        public string TaskAssignTypeName { get; set; }
    }
}
