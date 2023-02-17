using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProjectActivitiesViewModel
    {
        public Guid TaskTransitionLogId { get; set; }
        //Từ trạng thái
        [Display(Name = "Từ trạng thái")]
        public string FromStatusName { get; set; }
        //Đến trạng thái
        [Display(Name = "% Xác suất")]
        public string ToStatusName { get; set; }
        //Nội dung
        [Display(Name = "Nội dung")]
        public string Note { get; set; }
        //Người duyệt
        [Display(Name = "Người thực hiện")]
        public string ApproveName { get; set; }
        //Ngày duyệt
        [Display(Name = "Ngày thực hiện")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ApproveTime { get; set; }
        //Người tạo
        [Display(Name = "Người tạo")]
        public string CreateName { get; set; }
        //Ngày tạo
        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? CreateTime { get; set; }
    }
}
