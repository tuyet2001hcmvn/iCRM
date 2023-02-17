using ISD.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerSaleOrderViewModel
    {
        [Display(Name = "Mã đơn hàng")]
        public string SONumber { get; set; }

        [Display(Name = "Mã lệnh")]
        public string OrderNumber { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Số lượng")]
        [DisplayFormat(DataFormatString = SystemConfig.QuantityFormat)]
        public decimal? ProductQuantity { get; set; }
    }
}
