using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskWarrantyNVKTReportExcelModel
    {
        [Display(Name = "NV được phân công")]
        public string SalesEmployeeName { get; set; }
        [Display(Name = "Loại bảo hành")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Đã thực hiện")]
        public int QtyComplete { get; set; }
        [Display(Name = "Lịch hủy")]
        public int QtyCancel { get; set; }
        [Display(Name = "Tư vấn qua điện thoại ")]
        public int QtyAdvisoryPhone { get; set; }
        [Display(Name = "Lịch hẹn lại")]
        public int QtyBookLater { get; set; }

        [Display(Name = "Tổng cộng")]
        public int QtyTotal { get; set; }
        [Display(Name = "Tỷ lệ hoàn thành %")]
        public decimal CompleteRate { get; set; }

    }
}
