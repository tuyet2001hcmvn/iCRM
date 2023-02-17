using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CreditAuthenticateViewModel
    {
        public string CapDuyet { get; set; }
        public string UserNo { get; set; }
        public string Password { get; set; }
    }

    public class CreditAuthenticateInfoViewModel
    {
        //Mã User
        public string UserNo { get; set; }
        //Tên User
        public string UserName { get; set; }
        //Email From đóng vai trò là người gửi khi duyệt/bỏ duyệt
        public string FromEmail { get; set; }
        //Email To đóng vai trò là người được nhận khi duyệt/bỏ duyệt
        public string ToEmail { get; set; }
        //1.Thành Công ; 0.Thất bại
        public int? MessageNo { get; set; }
        //Chuỗi thông báo diễn giải login OK hay No OK cho user biết
        public string MessageName { get; set; }
    }
}
