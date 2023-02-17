using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskGroupReportViewModel
    {
        [Display(Name = "STT")]
        public int? NumberIndex { get; set; }

        [Display(Name = "Mã")]
        public string TaskCode { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Summary { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Mã")]
        public string TaskCodeSubTask { get; set; }

        [Display(Name = "Nhân viên được phân công")]
        public string AssigneeName { get; set; }

        [Display(Name = "Trạng thái CV con")]
        public string TaskStatusNameSubtask { get; set; }

        [Display(Name = "Mô tả (Subtask)")]
        public string DescriptionSubtask { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Ngày đến hạn")]
        public DateTime? EstimateEndDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Người tạo")]
        public string CreateByName { get; set; }

        [Display(Name = "Trang thái Giao việc Chính")]
        public string TaskStatusName { get; set; }

        [Display(Name = "Tên Nhóm Giao việc")]
        public string RolesName { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

    }
    public class TaskGroupReportSerchViewModel
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Contact")]
        public Nullable<System.Guid> CompanyId { get; set; }

        public string CompanyName { get; set; }
        //Ngày ghé thăm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }
        public string OrderBy { get; set; }
        public string TypeSort { get; set; }
        //Mô tả
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        //5a. Trạng thái (Guid)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public List<string> TaskStatusCode { get; set; }

        //5b. Nhóm trạng thái (string)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskProcessCode")]
        public string TaskProcessCode { get; set; }

        //6. Người giao việc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Reporter")]
        public List<string> Reporter { get; set; }
        //7. Nhân viên được phân công
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Assignee")]
        public List<string> Assignee { get; set; }

        //10. Người tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }

        //13. Ngày bắt đầu
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }

        //14. Ngày kết thúc dự kiến
        public string EstimateEndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EstimateEndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EstimateEndToDate { get; set; }

        //15. Ngày kết thúc
        public string EndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }
        //10. Người tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public List<string> CreateBy { get; set; }
        //Ngày tạo
        public string CreatedCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? CreatedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? CreatedToDate { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> RolesCode { get; set; }

        public string ReportType { get; set; }
    }
}
