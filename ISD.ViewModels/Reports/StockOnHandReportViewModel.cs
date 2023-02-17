using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockOnHandReportViewModel
    {
        [Display(Name = "Mã CTL")]
        public string ERPProductCode { get; set; }


        [Display(Name = "Nhoms Catalogue")]
        public string ProductGroups { get; set; }

        [Display(Name = "Tên Catalogue")]
        public string ProductName { get; set; }

        [Display(Name = "Nhóm vật tư")]
        public string CategoryName { get; set; }

        [Display(Name = "Mã kho xuất")]
        public string StockCode { get; set; }

        [Display(Name = "Tên kho xuất")]
        public string StockName { get; set; }

        [Display(Name = "Số lượng tồn")]
        public int? Qty { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; }

        [Display(Name = "Đơn giá")]
        public decimal? UnitPrice { get; set; }
    }
}
