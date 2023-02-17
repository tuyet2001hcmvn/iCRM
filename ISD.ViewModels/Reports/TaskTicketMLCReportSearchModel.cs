using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskTicketMLCReportSearchModel
    {
        //4. Loại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public List<Guid> WorkFlowId { get; set; }

        //5a. Trạng thái (Guid)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public string TaskStatusCode { get; set; }

        //5b. Nhóm trạng thái (string)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskProcessCode")]
        public string TaskProcessCode { get; set; }

        //7. Nhân viên được phân công
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Assignee")]
        public string Assignee { get; set; }

        //8. Khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public Nullable<System.Guid> ProfileId { get; set; }

        public string ProfileName { get; set; }

        //10. Người tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }

        //12. Ngày tiếp nhận
        public string ReceiveCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? ReceiveFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ReceiveToDate { get; set; }

        //13. Ngày bắt đầu
        public string StartCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }

        //15. Ngày kết thúc
        public string EndCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

        //21. Trung tâm bảo hành
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public List<string> ServiceTechnicalTeamCode { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        //public string CommonDate { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> DepartmentCode { get; set; }

        //Ngày tạo
        public string CreatedCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? CreatedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? CreatedToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerResult")]
        public string Property5 { get; set; }
        public bool IsView { get; set; }
    }
}