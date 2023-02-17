using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileWithPersonInChargeReportViewModel
    {
        public Guid? ProfileId { get; set; }

        [Display(Name = "Nhân viên kinh doanh")]
        public string SalesEmployeeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string RolesName { get; set; }

        [Display(Name = "Mã CRM")]
        public int? ProfileCode { get; set; }

        [Display(Name = "Mã SAP")]
        public string ProfileForeignCode { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string ProfileName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "SĐT")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Thị trường")]
        public string ForeignCustomer { get; set; }
        public bool? isForeignCustomer { get; set; }

        [Display(Name = "Phân loại KH")]
        public string CustomerTypeName { get; set; }

        [Display(Name = "Nhóm KH")]
        public string CustomerGroupName { get; set; }

        [Display(Name = "Ngành nghề")]
        public string CustomerCareerName { get; set; }

        [Display(Name = "Phường/Xã")]
        public string WardName { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string DistrictName { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Doanh thu năm trước đó")]
        public decimal? LastYearrevenue { get; set; }

        [Display(Name = "Doanh thu năm hiện tại")]
        public decimal? CurrentRevenue { get; set; }
    }

    public class ProfileWithPersonInChargeReportSearchViewModel
    {
        //Nhân viên kinh doanh
        [Display(Name = "Nhân viên kinh doanh")]
        public List<string> SalesEmployeeCode { get; set; }

        [Display(Name = "Phòng ban")]
        public List<string> DepartmentCode { get; set; }
        public bool IsView { get; set; }
    }
}
