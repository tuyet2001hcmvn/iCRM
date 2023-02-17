using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ActivitiesBySalesEmployeeReportViewModel
    {
        [Display(Name = "STT")]
        public long? NumberIndex { get; set; }

        [Display(Name = "Mã NV")]
        public string SalesEmployeeCode { get; set; }

        [Display(Name = "NV kinh doanh")]
        public string SalesEmployeeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string RolesName { get; set; }

        [Display(Name = "Loại hoạt động")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Tên khách")]
        public string ProfileName { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Summary { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public string TaskStatusName { get; set; }

        [Display(Name = "Ngày thực hiện")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "Số hoạt động")]
        public int? CountActivities { get; set; }
    }

    public class ActivitiesBySalesEmployeeReportSearchViewModel
    {
        [Display(Name = "NV kinh doanh")]
        public string SalesEmployeeCode { get; set; }

        [Display(Name = "Loại hoạt động")]
        public Guid? WorkFlowIdList { get; set; }
        public List<string> SalesEmployeeCodes { get; set; }
        public List<Guid> WorkFlowIdLists { get; set; }

        [Display(Name = "Ngày thực hiện")]
        public string CommonDate { get; set; }

        [Display(Name = "Từ ngày")]
        public DateTime? StartFromDate { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? StartToDate { get; set; }

        [Display(Name = "Ngày tạo")]
        public string CommonDate2 { get; set; }

        [Display(Name = "Từ ngày")]
        public DateTime? CreateFromDate { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? CreateToDate { get; set; }

        public bool IsView { get; set; }
    }
}
