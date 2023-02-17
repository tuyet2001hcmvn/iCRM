using ISD.Constant;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class RevenueViewModel
    {
        public int? STT { get; set; }

        //Công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string WERKS { get; set; }

        //Chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string BUKRS { get; set; }

        //Mã khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string KUNNR { get; set; }

        //Tên khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CUSTNAME { get; set; }

        //Địa chỉ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string CUSTADDR { get; set; }

        //Điện thoại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string CUSTPHONE { get; set; }

        //Mã số thuế
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string STCEG { get; set; }

        //Doanh số
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Revenue")]
        public decimal? DOANHSO { get; set; }

        //Cấp bậc khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CurrentCustomerLevel")]
        public string CustomerLevel { get; set; }

        //Hạn mức hiện tại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CurrentLimit")]
        public decimal? CurrentLimit { get; set; }

        //Giá trị quy đổi
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExchangeValue")]
        public Nullable<decimal> ExchangeValue { get; set; }

        //Điểm tích lũy
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Point")]
        public decimal? Point { get; set; }

        //Giá trị thiếu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MissingValue")]
        public decimal? MissingValue { get; set; }

        //Hạn mức kế tiếp
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NextLimit")]
        public decimal? NextLimit { get; set; }

        //Nhóm khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
        public string CUSTGROUP { get; set; }

        //Quận/Huyện
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public string SALEDISTRICT { get; set; }

        //YEARMONTH
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Year")]
        public string YEARMONTH { get; set; }
    }

    public class RevenueSearchViewModel
    {
        //Công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? CompanyId { get; set; }

        //Mã khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        //Điện thoại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string PhoneNumber { get; set; }

        //Từ hạn mức
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromLimit")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? FromLimit { get; set; }

        //Đến hạn mức
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToLimit")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? ToLimit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberOfRows")]
        public int? NumberOfRows { get; set; }
    }
}