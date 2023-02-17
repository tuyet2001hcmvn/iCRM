using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerCareActivitiesViewModel
    {
        [Display(Name = "STT")]
        public long? NumberIndex { get; set; }
        [Display(Name = "Nhân viên")]
        public string SalesEmployeeName { get; set; }
        [Display(Name = "Phòng Ban")]
        public string DepartmentName { get; set; }
        [Display(Name = "Ngày thực hiện")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Khách ghé thăm")]
        public int? AppointmentQty { get; set; }
        [Display(Name = "Spec, Chăm sóc KH")]
        public int? THKH_SpecKHQty { get; set; }
        [Display(Name = "Khảo sát-Lắp đặt ĐTB")]
        public int? THKH_KhaoSatLdDTBQty { get; set; }
        [Display(Name = "Chăm sóc ĐTB")]
        public int? THKH_SpecDTBQty { get; set; }
        [Display(Name = "Thăm hỏi khác")]
        public int? THKH_Khac { get; set; }
        [Display(Name = "Xư lý Khiếu nại")]
        public int? TICKET_XLKNQty { get; set; }
        [Display(Name = "Kiểm Ván")]
        public int? TICKET_KVQty { get; set; }
        [Display(Name = "Khảo sát")]
        public int? TICKET_KSQty { get; set; }
        [Display(Name = "Lắp đặt")]
        public int? TICKET_LDQty { get; set; }
        [Display(Name = "Bảo hành")]
        public int? TICKET_BHQty { get; set; }
        [Display(Name = "HDSD")]
        public int? TICKET_HDSDQty { get; set; }
        [Display(Name = "Gửi Email")]
        public int? ACTIVITIES_GTDTQty { get; set; }

        [Display(Name = "Gọi điện")]
        public int? ACTIVITIES_GDQty { get; set; }

        [Display(Name = "Gửi CTL/Mẫu")]
        public int? ACTIVITIES_GCTLQty { get; set; }

        [Display(Name = "Nhiệm vụ khác")]
        public int? ACTIVITIES_KCQty { get; set; }

        [Display(Name = "Giao Việc")]
        public int? MissionQty { get; set; }
        [Display(Name = "Số Nhiệm vụ")]
        public int? ActivitiesQty { get; set; }
        [Display(Name = "Tổng các hoạt động")]
        public int? Total { get; set; }

       



        //Mobile
        public int? QtyGheTham { get; set; }
        public int? QtyTHKH { get; set; }
        public int? QtyBaoHanh { get; set; }
        public int? QtyXLKN { get; set; }
        public int? QtyNhiemVu { get; set; }
    }

    public class CustomerCareSearchViewModel
    {
        [Display(Name = "Công ty")]
        public Guid? CompanyId { get; set; }
        [Display(Name = "Phòng ban")]
        public List<Guid> DepartmentId { get; set; }
        [Display(Name = "Nhân viên")]
        public List<string> SalesEmployeeCode { get; set; }

        [Display(Name = "Loại hoạt động")]
        public List<string> WorkFlowCategoryCode { get; set; }
        //13. Ngày bắt đầu
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }

        //Phân loại chuyến thăm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_VisitTypeCode")]
        public string VisitTypeCode { get; set; }

        [Display(Name = "Chế độ xem")]
        public bool ViewTotal { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public string TaskStatusCode { get; set; }
        public bool IsView { get; set; }
    }
}
