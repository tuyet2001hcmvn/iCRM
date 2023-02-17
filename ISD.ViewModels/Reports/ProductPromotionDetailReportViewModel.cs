using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionDetailReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public int ProfileCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileAddress")]
        public string ProfileAddress { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_ProfileContacts")]
        public string ProfileContact { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_Title")]
        public string ProductPromotionTitle { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_SendTypeCode")]
        public string SendTypeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string Status { get; set; }

    }
    public class ProductPromotionDetailReportSearchViewModel
    {

        [Display(Name = "Nhân viên kinh doanh")]
        public List<string> SalesEmployeeCode { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        public List<string> CheckerCode { get; set; }
        [Display(Name = "Khách hàng")]
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
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
