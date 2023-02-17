using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileExportExcelModel
    {
        [Display(Name = "Mã KH trên CRM")]
        public string ProfileCode { get; set; }

        [Display(Name = "Nhóm khách hàng")]
        public string isForeignCustomer { get; set; }
        [Display(Name = "Công ty")]
        public string CreateAtCompany { get; set; }
        [Display(Name = "Vai trò trong giao dịch")]
        public string PartnerFunctions { get; set; }
        [Display(Name = "Tổ chức bán hàng")]
        public string SaleOrg { get; set; }
        [Display(Name = "Cá nhân/Tổ chức")]
        public string Title { get; set; }
        [Display(Name = "Tên ngắn")]
        public string ProfileShortName { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string ProfileName { get; set; }
        [Display(Name = "Địa chỉ của khách hàng")]
        public string Address { get; set; }
        [Display(Name = "Quốc gia")]
        public string Country { get; set; }
        [Display(Name = "Mã số thuế")]
        public string TaxNo { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Email công ty")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ email nhận HĐ điện tử")]
        public string Email2 { get; set; }
        [Display(Name = "Loại KH")]
        public string CustomerTypeCode { get; set; }
        [Display(Name = "Ngành nghề KH")]
        public string CustomerCareerCode { get; set; }
        [Display(Name = "Tài khoản công nợ")]
        public string ReconAccount { get; set; }
        [Display(Name = "Điều khoản thanh toán")]
        public string PaymentTerms { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethods { get; set; }
        [Display(Name = "Khu vực bán hàng")]
        public string SalesDistrict { get; set; }
        [Display(Name = "Thị trường cấp 1")]
        public string SaleOfficeCode { get; set; }
        [Display(Name = "Thị trường cấp 2")]
        public string SaleGroupCode { get; set; }
        [Display(Name = "Nhóm KH")]
        public string CustomerGroupCode { get; set; }
        [Display(Name = "Đơn vị tiền tệ")]
        public string Currency { get; set; }
        [Display(Name = "Tài khoản doanh thu")]
        public string AccountAssignmentGrp { get; set; }
        [Display(Name = "Phân loại thuế VAT")]
        public string TaxClassification { get; set; }
        [Display(Name = "Mã nhân viên quản lý")]
        public string ManagerCode { get; set; }
        [Display(Name = "Nhân viên quản lý")]
        public string Manager { get; set; }
        [Display(Name = "Nhân viên kinh doanh")]
        public string SaleEmployee { get; set; }
        [Display(Name = "Mã nhân viên công nợ")]
        public string DebsEmployeeCode { get; set; }
        [Display(Name = "Nhân viên công nợ")]
        public string DebsEmployee { get; set; }
        [Display(Name = "Phòng ban quản lý")]
        public string ManagementDepartment { get; set; }

        [Display(Name = "Tên công ty")]
        public string ContactCompany { get; set; }
        [Display(Name = "Tên ngắn người liên hệ")]
        public string ContactShortName { get; set; }
        [Display(Name = "Tên đầy đủ người liên hệ")]
        public string ContactName { get; set; }
        [Display(Name = "Phòng ban")]
        public string ContactDepartment { get; set; }
        [Display(Name = "Chức vụ")]
        public string ContactFunction { get; set; }
        [Display(Name = "ĐT cố định")]
        public string ContactTelephone { get; set; }
        [Display(Name = "Ext ĐT cố định")]
        public string ContactExtTelephone { get; set; }
        [Display(Name = "ĐT di động")]
        public string ContactMobilephone { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "Địa chỉ")]
        public string ContactAddress { get; set; }
        public Guid ProfileId { get; set; }
    }
}
