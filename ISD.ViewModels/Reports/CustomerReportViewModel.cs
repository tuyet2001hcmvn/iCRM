using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Reports
{
    public class CustomerReportViewModel
    {
        //Từ ngày
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime FromDate { get; set; }

        //Đến ngày
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime ToDate { get; set; }

        //Mã nhân viên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_PersonnalNumberId")]
        public string EmployeeCode { get; set; }

        //Cơ hội bán hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Opportunities")]
        public bool isOpportunity { get; set; }
    }

    public class CustomerReportResultViewModel
    {
        public int STT { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string MaNhanVien { get; set; }
        public string CoHoiBanHang { get; set; }
    }
}
