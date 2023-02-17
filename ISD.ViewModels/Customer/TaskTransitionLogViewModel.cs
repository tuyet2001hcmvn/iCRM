using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskTransitionLogViewModel
    {

        public Guid TaskTransitionLogId { get; set; }

        public Guid? TaskId { get; set; }

        [Display(Name = "Xác suất hiện tại")]
        public Nullable<System.Guid> FromStatusId { get; set; }

        [Display(Name = "Xác suất chuyển đến")]
        public Nullable<System.Guid> ToStatusId { get; set; }

        [Display(Name = "Nội dung")]
        public string Note { get; set; }

        [Display(Name = "Người thực hiện")]
        public Guid? ApproveBy { get; set; }

        [Display(Name = "Ngày thực hiện")]
        public DateTime? ApproveTime { get; set; }

        [Display(Name = "Người tạo")]
        public Guid? CreateBy { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

    }
}
