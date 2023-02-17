using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ShowroomReportViewModel
    {
        [Display(Name = "Loại")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Trạng thái")]
        public string TaskStatusName { get; set; }

        [Display(Name = "Khu vực")]
        public string Area { get; set; }

        [Display(Name = "Số lượng")]
        public decimal? NumberOfShowroom { get; set; }

        [Display(Name = "Giá trị")]
        public decimal? ValueOfShowroom { get; set; }
    }

    public class ShowroomReportSearchViewModel
    {
        [Display(Name = "Công ty")]
        public Guid? CompanyId { get; set; }

        [Display(Name = "Loại")]
        public List<Guid> WorkFlowId { get; set; }

        [Display(Name = "Trạng thái")]
        public List<string> TaskStatusCode { get; set; }

        [Display(Name = "Khu vực")]
        public List<string> Area { get; set; }

        [Display(Name = "Ngày thực hiện")]
        public string CommonDate { get; set; }

        [Display(Name = "Từ ngày")]
        public DateTime? StartFromDate { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? StartToDate { get; set; }

        [Display(Name = "Nhóm vật tư")]
        public List<Guid> CategoryId { get; set; }
        public bool IsView { get; set; }

        public string ReportType { get; set; }
    }
}
