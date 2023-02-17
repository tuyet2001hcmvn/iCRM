using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionSearchViewModel
    {

        public string Type { get; set; }
        [Display(Name = "Tiêu đề")]
        public string ProductPromotionTitle { get; set; }
        [Display(Name = "Phương thức gửi")]
        public string SendTypeCode { get; set; }
        [Display(Name = "Khách hàng")]
        public Guid? ProfileId { get; set; }
        [Display(Name = "Thời gian bắt đầu")]
        public string StartTime { get; set; }
        [Display(Name = "Thời gian kết thúc")]
        public string EndTime { get; set; }
        [Display(Name = "Từ")]
        public Nullable<System.DateTime> FromStartTime { get; set; }
        [Display(Name = "Đến")]
        public Nullable<System.DateTime> FromEndTime { get; set; }
        [Display(Name = "Từ")]
        public Nullable<System.DateTime> ToStartTime { get; set; }
        [Display(Name = "Đến")]
        public Nullable<System.DateTime> ToEndTime { get; set; }
        [Display(Name = "Trạng thái")]
        public bool? Actived { get; set; }
    }
}
