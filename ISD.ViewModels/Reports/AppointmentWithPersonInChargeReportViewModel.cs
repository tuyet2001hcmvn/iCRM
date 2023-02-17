using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AppointmentWithPersonInChargeReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }

        [Display(Name = "Mã nhân viên")]
        public string SalesEmployeeCode { get; set; }

        [Display(Name = "NV Kinh doanh")]
        public string SalesEmployeeName { get; set; }

        [Display(Name = "Số lượng")]
        public int QtyAppointment { get; set; }

        [Display(Name = "SL kì trước")]
        public int NumberOfPrevious{ get; set; }

        [Display(Name = "Tỷ lệ")]
        public string Ratio { get; set; }
    }
    public class AppointmentWithPersonInChargeReportSearchViewModel
    {
        [Display(Name = "Nhân viên kinh doanh")]
        public List<string> SalesEmployeeCode { get; set; }

        [Display(Name = "Phòng ban")]
        public List<string> RolesCode { get; set; }


        //Thời gian kỳ này
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? StartFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? StartToDate { get; set; }
        //Thời gian kỳ trước
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string EndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? EndFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? EndToDate { get; set; }
        public bool IsView { get; set; }
    }
}
