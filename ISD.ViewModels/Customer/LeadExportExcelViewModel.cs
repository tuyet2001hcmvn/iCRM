using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class LeadExportExcelViewModel
    {
        [Display(Name = "Tên dự án")]
        public string ProfileName { get; set; }

        [Display(Name = "Tên chủ đầu tư")]
        public string Investor { get; set; }

        [Display(Name = "Thiết kế")]
        public string Design { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }

        [Display(Name = "Loại hình")]
        public string OpportunityType { get; set; }

        [Display(Name = "Quy mô")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? ProjectGabarit { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string OpportunityUnit { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "NV cập nhật")]
        public string LastEditByName { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }


    }
}
