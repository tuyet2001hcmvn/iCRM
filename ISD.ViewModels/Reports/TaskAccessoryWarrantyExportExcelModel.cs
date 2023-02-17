using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskAccessoryWarrantyExportExcelModel
    {
        [Display(Name = "Mã SAP Phụ kiện")]
        public string ERPAccessoryCode { get; set; }
        [Display(Name = "Tên phụ kiện")]
        public string AccessoryName { get; set; }
        [Display(Name = "Loại phụ kiện")]
        public string AccessoryTypeName { get; set; }
        [Display(Name = "Hình thức bảo hành")]
        public string ErrorTypeName { get; set; }
        [Display(Name = "Số lượng")]
        public int? AccessoryQuantity { get; set; }

    }
}
