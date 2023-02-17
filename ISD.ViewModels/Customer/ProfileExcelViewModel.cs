using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileExcelViewModel
    {
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileId")]
        //public System.Guid ProfileId { get; set; }
        [Display(Name = "Thị trường")]
        [Required]
        public bool? isForeignCustomer { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerType")]
        [Required]
        public string CustomerTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerSourceCode")]
        public string CustomerSourceCode { get; set; }

        [Display(Name = "Mã công ty (*)")]
        public string CreateAtCompany { get; set; }

        [Display(Name = "Chi nhánh (*)")]
        public string CreateAtSaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Title")]
        //[Required]
        public string Title { get; set; }

        [Display(Name = "Tên KH")]
        [Required]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileShortName")]
        public string ProfileShortName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Age")]
        public string Age { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }

        [Display(Name = "SDT (*)")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
        public string Email { get; set; }

        public string Website { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public string SaleOfficeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_AddressTypeCode")]
        public string AddressTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public Guid? ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public Guid? DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public Guid? WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }

        [Display(Name = "Ngành nghề")]
        public string CustomerCareerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "NV phụ trách (*)")]
        public string PersonInCharge { get; set; }

        [Display(Name = "Tên người liên hệ")]
        public string ContactName { get; set; }

        [Display(Name = "SDT liên hệ (*)")]
        public string ContactNumber { get; set; }

        [Display(Name = "Email liên hệ")]
        public string ContactEmail { get; set; }

        [Display(Name = "Phòng ban")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Chức vụ")]
        public string Position { get; set; }

        [Display(Name = "Nhóm khách hàng")]
        public string CustomerGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileGroup_CreateBy")]
        public string CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileGroup_CreateTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy HH:mm}")]
        public DateTime? CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}