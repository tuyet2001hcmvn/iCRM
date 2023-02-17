using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskTicketMLCReportExcelModel
    {
        [Display(Name = "Ngày tiếp nhận")]
        public DateTime? ReceiveDate { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateByName { get; set; }
        [Display(Name = "Khách hàng")]
        public string ProfileName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string ProfileAddress { get; set; }
        [Display(Name = "SĐT liên hệ")]
        public string Phone { get; set; }
        [Display(Name = "Loại")]
        public string WorkFlowName { get; set; }
        [Display(Name = "Trạng thái")]
        public string TaskStatusName { get; set; }
        [Display(Name = "Trung tâm bảo hành")]
        public string ServiceTechnicalName { get; set; }
        [Display(Name = "NV được phân công")]
        public string TaskAssigneeName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Kết quả")]
        public string CustomerReviews { get; set; }
        [Display(Name = "Đánh giá chất lượng dịch vụ")]
        public string ServiceRating { get; set; }
        [Display(Name = "Đánh giá chất lượng sản phẩm")]
        public string ProductRating { get; set; }
        [Display(Name = "Ý kiến khách hàng")]
        public string Property5 { get; set; }
        public Guid ProfileId { get; set; }


    }
}
