using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class FindVehicleDTO
    {
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Số sổ bảo hành")]
        public string SoBH { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKH { get; set; }

        [Display(Name = "Số khung")]
        public string SoKhung { get; set; }

        [Display(Name = "Biển số xe")]
        public string LicensePlate { get; set; }
    }
}
