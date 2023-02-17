using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerGroupReportViewModel
    {
        [Display(Name = "STT")]
        public int? STT { get; set; }

        [Display(Name = "Nhóm KH")]
        public string NhomKH { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        public string CustomerGroupCode { get; set; }

    }
}
