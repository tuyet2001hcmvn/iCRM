using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TechnicalServicesReportSearchViewModel
    {
        [Display(Name = "Trạng thái")]
        public List<string> TaskStatusCode { get; set; }
        [Display(Name = "Phòng ban")]
        public List<string> RolesCode { get; set; }
        [Display(Name = "Trung tâm bảo hành")]
        public List<string> ServiceTechnicalTeamCode { get; set; }
        [Display(Name = "Loại")]
        public List<Guid> WorkFlowId { get; set; }
        [Display(Name = "Thời gian")]
        public string CommonDate { get; set; }
        [Display(Name = "Từ ngày")]
        public DateTime? FromDate { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }
        public bool? IsCallFirst { get; set; }
    }
}
