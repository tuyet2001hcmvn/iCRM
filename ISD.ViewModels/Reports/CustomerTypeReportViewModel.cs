using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerTypeReportViewModel
    {
        [Display(Name = "STT")]
        public int? STT { get; set; }

        [Display(Name = "Phân loại KH")]
        public string PhanLoaiKH { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        public string CustomerTypeCode { get; set; }
    }
}
