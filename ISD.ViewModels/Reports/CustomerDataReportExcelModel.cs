using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerDataReportExcelModel
    {
        [Display(Name = "Mã SAP khách hàng")]
        public string ProfileForeignCode { get; set; }
        [Display(Name = "Tên khách")]
        public string ProfileName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Đơn hàng bán")]
        public string SaleOrderCode { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }
        [Display(Name = "Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }
        [Display(Name = "Quận/Huyện")]
        public string DistrictName { get; set; }
    }
}
