using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warranty.Models
{
    public class RegWarrantyCheckModel
    {
        [Display(Name = "Số serial")]
        public string SerialCheck { get; set; }

        [Display(Name = "Số OD")]
        public string OrderDelivery { get; set; }

        [Display(Name = "Nhập mã xác nhận")]
        public string CaptchaCodeCheck { get; set; }

        public string WarrantyType { get; set; }
    }
}