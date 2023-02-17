using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion")]
        public string ProductPromotionTitle { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesName")]
        public string RolesName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployeeName")]
        public string SalesEmployeeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployeeName")]
        public int TotalCustomer { get; set; }
        [Display(Name = "Đã gửi")]
        public int Sent { get; set; }
        [Display(Name = "Chưa gửi")]
        public int Unsent { get {
                return TotalCustomer - Sent;
            } }

    }
    public class ProductPromotionReportSearchViewModel
    {
        [Display(Name = "Khách hàng")]
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        [Display(Name = "Nhân viên gửi")]
        public List<string> CheckerCode { get; set; }
        [Display(Name = "Phòng ban")]
        public List<string> RolesCode { get; set; }
        [Display(Name = "Thời gian gửi")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }

        public bool IsView { get; set; }
    }
}
