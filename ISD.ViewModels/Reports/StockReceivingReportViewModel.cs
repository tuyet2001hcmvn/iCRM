using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockReceivingReportViewModel
    {
        [Display(Name = "Ngày chứng từ")]
        public DateTime? DocumentDate { get; set; }
        [Display(Name = "Mã phiếu nhập")]
        public string StockReceivingCode { get; set; }
        [Display(Name = "Mã Catalogue")]
        public string ERPProductCode { get; set; }
        [Display(Name = "Nhóm Catalogue")]
        public string ProductGroups { get; set; }
        [Display(Name = "Tên Catalogue")]
        public string ProductName { get; set; }
        [Display(Name = "Nhóm Vật Tư")]
        public string CategoryName { get; set; }
        [Display(Name = "Số lượng nhập")]
        public decimal? Quantity { get; set; }
        [Display(Name = "Mã Kho Nhận")]
        public string StockCode { get; set; }
        [Display(Name = "Tên Kho nhận")]
        public string StockName { get; set; }
        [Display(Name = "Nhân viên")]
        public string SalesEmployeeName { get; set; }
        [Display(Name = "Phòng ban")]
        public string DepartmentName { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
       

    }
}
