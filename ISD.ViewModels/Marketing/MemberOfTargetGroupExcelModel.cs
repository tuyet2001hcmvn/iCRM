using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MemberOfTargetGroupExcelModel
    {
        [Display(Name = "Mã CRM")]
        public int ProfileCode { get; set; }

        [Display(Name = "Tên KH")]
        public string ProfileName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
