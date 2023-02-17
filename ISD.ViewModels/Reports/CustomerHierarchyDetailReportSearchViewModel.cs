using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerHierarchyDetailReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }

        [Display(Name = "Mã KH")]
        public string ProfileCode { get; set; }

        [Display(Name = "Mã SAP")]
        public string ProfileForeignCode { get; set; }

        [Display(Name = "Tên KH")]
        public string ProfileName { get; set; }

        [Display(Name = "Doanh số")]
        public decimal? Value { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Adresses { get; set; }

        [Display(Name = "Nhân viên")]
        public string SalesEmployeeName { get; set; }

        [Display(Name = "Nhóm KH")]
        public string CustomerGroupName { get; set; }

    }
    public class CustomerHierarchyDetailReportSearchViewModel
    {
      
        public bool IsView { get; set; }

        [Display(Name = "Công ty")]
        public string CompanyCode { get; set; }

        [Display(Name = "Phòng ban")]
        public List<string> RolesCode { get; set; }

        [Display(Name = "Nhân viên")]
        public List<string> SalesEmployeeCode { get; set; }

        [Display(Name = "Khách hàng")]
        public List<string> ProfileForeignCode { get; set; }
        [Display(Name = "Thời gian")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Nhóm khách hàng")]
        public List<string> CustomerGroupCode { get; set; }

        [Display(Name = "Khu vực")]
        public List<string> SaleOfficeCode { get; set; }

        [Display(Name = "Tỉnh/Thành")]
        public List<string> ProvinceCode { get; set; }

        [Display(Name = "Quận/Huyện")]
        public List<string> DistrictCode { get; set; }

        [Display(Name = "Ngành nghề")]
        public List<string> CustomerCareerCode { get; set; }
        [Display(Name = "Nhóm doanh số")]
        public List<string> GroupValueCode { get; set; }
        [Display(Name = "Từ")] 
        public decimal? FromValue { get; set; }
        [Display(Name = "Đến")] 
        public decimal? ToValue { get; set; }

        public string ReportType { get; set; }



    }
}
