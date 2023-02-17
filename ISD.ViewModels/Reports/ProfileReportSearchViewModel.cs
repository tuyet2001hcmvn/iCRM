using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileReportSearchViewModel
    {
        /// 1. Ngày nhập (từ … đến …)
        /// 
        //Từ ngày
        public string FromDate { get; set; }
        //Đến ngày
        public string ToDate { get; set; }
        /// 2. User
        public Guid? UserName { get; set; }
        /// 3. Nguồn KH
        public string CustomerSourceCode { get; set; }
        /// 4. Phân loại KH(B/C)
        public string CustomerTypeCode { get; set; }
        /// 5. Nhóm KH
        public string CustomerGroupCode { get; set; }
        /// 6. Ngành nghề KH
        public string CustomerCareerCode { get; set; }
        /// 7. Trạng thái xử lý yêu cầu KH
        public Guid? TaskStatusId { get; set; }

        #region //Additional Field by login user
        //Công ty
        public string CurrentCompanyCode { get; set; }
        //Chi nhánh
        public string CurrentSaleOrg { get; set; }
        //Tài khoản đang đăng nhập
        public string CurrentUserName { get; set; }
        //Có phải là đại lý ủy quyền hay không
        public bool? isAgency { get; set; }
        #endregion
    }

    public class ProfileReportPivotSearchViewModel
    {
        public bool IsView { get; set; }
        public string ReportType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileName")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public List<Guid> DistrictId { get; set; }
        public bool? DistrictIdIsNull { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressTypeCode")]
        public List<string> AddressTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public List<System.Guid> WardId { get; set; }
        public bool? WardIdIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerTypeCode { get; set; }
        public bool? CustomerTypeCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }

        public bool? AddressIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        public string Type { get; set; }
        public string ProfileType { get; set; }

        public string Email { get; set; }
        public bool? EmailIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Guid? CreateUser { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        //Mã khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileCode")]
        public string ProfileCode { get; set; }

        //Mã SAP khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public string ProfileForeignCode { get; set; }

        public bool? hasNoContact { get; set; }

        public Guid? ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public List<Guid> ProvinceId { get; set; }
        public List<Guid> ProvinceIdList { get; set; }
        public bool? ProvinceIdIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyId")]
        public Nullable<System.Guid> CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public List<Guid?> StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Age")]
        public string Age { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public string SearchProfileForeignCode { get; set; }
        public bool? SearchProfileForeignCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
        public List<string> CustomerGroupCode { get; set; }
        public bool? CustomerGroupCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
        public string CustomerCareerCode { get; set; }
        public bool? CustomerCareerCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge6Code")]
        public string PersonInCharge6Code { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeCode { get; set; }
        public bool? SalesEmployeeCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> RolesCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateFromDate")]
        public DateTime? CreateFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateToDate")]
        public DateTime? CreateToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerSourceCode")]
        public string CustomerSourceCode { get; set; }
        public bool? CustomerSourceCodeIsNull { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }

        public bool? TaxNoIsNull { get; set; }

        public string CreateCommonDate { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerAccountGroup")]
        public List<string> CustomerAccountGroupCode { get; set; }

        public bool? CustomerAccountGroupAll { get; set; }

        //Yêu cầu tạo khách ở ECC
        public bool? isCreateRequest { get; set; }
        public string CreateRequestAll { get; set; }
        public DateTime? CreateRequestTimeFrom { get; set; }
        public DateTime? CreateRequestTimeTo { get; set; }

        //Khu vực
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public string SaleOfficeCode { get; set; }

        public string CompetitorType { get; set; }
        public string SearchCompetitorType { get; set; }
        public string FileName { get; set; }

        //chọn profile để lưu
        public Guid? ProfileIsChoosen { get; set; }

        public ProfileViewModel PartnerInfo { get; set; }

        public string SearchProfileName { get; set; }
    }
}
