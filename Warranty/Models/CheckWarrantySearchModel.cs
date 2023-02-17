using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warranty.Models
{
    public class CheckWarrantySearchModel
    {
        [Display(Name = "Số Serial")]
        public string SearchSeriNo { get; set; }
        [Display(Name = "Số điện thoại")]
        public string SearchPhone { get; set; }
        [Display(Name = "Số OD")]
        public string SearchOrderDelivery { get; set; }
        [Display(Name = "Nhập mã xác nhận")]
        public string CaptchaCode { get; set; }

        public string SearchType { get; set; }
        [Display(Name = "Mã OTP")]
        public string VerifyOTP { get; set; }
    }
}