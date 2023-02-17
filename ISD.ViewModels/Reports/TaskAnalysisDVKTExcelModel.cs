using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskAnalysisDVKTExcelModel
    {
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
        [Display(Name = "Đã thực hiện so với kỳ trước")]
        public string PreviousCompleteRate { get; set; }
        [Display(Name = "Lịch hủy so với kỳ trước")]
        public string PreviousCancelRate { get; set; }
        [Display(Name = "Tư vấn qua điện thoại so với kỳ trước")]
        public string PreviousAdvisoryPhoneRate { get; set; }
        [Display(Name = "Lịch hẹn lại so với kỳ trước")]
        public string PreviousBookLaterRate { get; set; }

        [Display(Name = "Tổng cộng so với kỳ trước")]
        public string PreviousTotalRate { get; set; }
    }
}
