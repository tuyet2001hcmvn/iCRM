using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class GH_NotificationViewModel
    {
        public Guid NotificationId { get; set; }
        
        public string NotificationUrl { get; set; }

        [Display(Name = "Chi nhánh")]
        public string SaleOrgName { get; set; }

        [Display(Name = "Nội dung thông báo")]
        public string NotificationContent { get; set; }

        [Display(Name = "Thời gian thông báo")]
        public DateTime? NotificationDateTime { get; set; }

        [Display(Name = "Đã đọc")]
        public bool? isComplete { get; set; }

        //Dữ liệu lấy từ SAP
        public string SaleOrderId { get; set; }
        public string SaleOrderCode { get; set; }
        public string SAP_SaleOrderCode { get; set; }
    }
}
