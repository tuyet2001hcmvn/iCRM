using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductAccessoryReportExcelModel
    {
        [Display(Name = "Mã SAP sản phẩm")]
        public string ERPProductCode { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Số lượng")]
        public int? ProductQuantity { get; set; }
        [Display(Name = "Phân loại sản phẩm")]
        public string ProductCategoryName { get; set; }
        [Display(Name = "Phương thức xử lý")]
        public string ErrorName { get; set; }
        [Display(Name = "Hình thức bảo hành")]
        public string ErrorTypeName { get; set; }
        [Display(Name = "Mã SAP Phụ kiện")]
        public string ERPAccessoryCode { get; set; }
        [Display(Name = "Tên phụ kiện")]
        public string AccessoryName { get; set; }
        [Display(Name = "Số lượng phụ kiện")]
        public int? AccessoryQuantity { get; set; }
        [Display(Name = "Loại phụ kiện")]
        public string AccessoryTypeName { get; set; }

    }
}
