using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class CheckingTimesNotificationViewModel
    {
        public Guid CheckingTimesId { get; set; }
        //Mã khách hàng
        public string CustomerCode { get; set; }
        //Nội dung thông báo kiểm tra định kỳ
        public string CheckingTimesDescription { get; set; }
        //Trạng thái đọc thông báo
        public bool? isRead { get; set; }
    }
}
