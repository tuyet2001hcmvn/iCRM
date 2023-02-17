using ISD.Constant;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class TaskExportVisitViewModel
    {
        public long? RowNum { get; set; }
        public Guid? TaskId { get; set; }
        [Display(Name = "ID")]
        public int? TaskCode { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Summary { get; set; }
        [Display(Name = "Mô tả")]
        public string Desriptions { get; set; }
        [Display(Name = "Loại")]
        public string WorkFlowName { get; set; }
        [Display(Name = "Trạng thái")]
        public string TaskStatusName { get; set; }
        [Display(Name = "Khách hàng")]
        public string ProfileName { get; set; }
        [Display(Name = "Liên hệ")]
        public string ContactName { get; set; }
        [Display(Name = "SĐT liên hệ")]
        public string ContactPhone { get; set; }
        [Display(Name = "Phương tiện")]
        public string Property1 { get; set; }
        [Display(Name = "Phụ trách tiếp khách")]
        public string AssigneeName { get; set; }
        [Display(Name = "Kiểm soát vào/ra")]
        public string ReporterName { get; set; }
        [Display(Name = "Nơi tham quan")]
        public string VisitPlace { get; set; }
        [Display(Name = "Comment tương tác")]
        public string Comment { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateByName { get; set; }
        [Display(Name = "Ngày giờ tạo lịch")]
        public DateTime? CreateTime { get; set; }
        [Display(Name = "Ngày giờ bắt đầu tham quan")]
        public DateTime? StartDateVisit { get; set; }
        [Display(Name = "Ngày giờ kết thúc tham quan")]
        public DateTime? EndDateVisit { get; set; }
        [Display(Name = "Người chuyển trạng thái vào")]
        public string INSalesEmployeeName { get; set; }
        [Display(Name = "Ngày giờ chuyển trạng thái vào")]
        public DateTime? INLastEditTime { get; set; }
        [Display(Name = "Người chuyển trạng thái ra")]
        public string OUTSalesEmployeeName { get; set; }
        [Display(Name = "Ngày giờ chuyển trạng thái ra")]
        public DateTime? OUTLastEditTime { get; set; }
       
    }
}
