using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileQuantityAppointmentWithShowRoomReportViewModel
    {
        [Display(Name = "Nguồn KH")]
        public string ShowroomName { get; set; }
        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Chi nhánh")]
        public string StoreName { get; set; }

        [Display(Name = "Số lượng")]
        public int ProfileCount { get; set; }

        [Display(Name = "Số lượng kì so sánh")]
        public decimal? ProfileCountPrevious { get; set; }

        [Display(Name = "Số lượng chênh lệch")]
        public int ProfileCountDifference { get; set; }

        [Display(Name = "Kì này/Kì trước")]
        public string Ratio { get; set; }
        public Guid? StoreId { get; set; }

    }

    public class ProfileQuantityAppointmentWithShowRoomReportSearchViewModel
    {

        //Showroom
        [Display(Name = "Chi nhánh")]
        public List<string> CreateAtSaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? StartFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? StartToDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string EndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? EndFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? EndToDate { get; set; }
        public bool? CompareDateTime { get; set; }
        public bool IsView { get; set; }
        public string ReportType { get; set; }
        [Display(Name = "Nguồn KH")]
        public string CustomerSource { get; set; }
    }
}
