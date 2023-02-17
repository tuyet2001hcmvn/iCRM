using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerCatalogueReportViewModel
    {
        [Display(Name = "STT")]
        public int? STT { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; }

        [Display(Name = "Mã catalog")]
        public string MaCatalogue { get; set; }

        [Display(Name = "Tên CTL")]
        public string TenCTL { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }
    }
}
