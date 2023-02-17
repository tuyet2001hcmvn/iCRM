using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class SalesEmployeeViewModel
    {
        //Mã nhân viên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyId")]
        public Nullable<System.Guid> CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Nullable<System.Guid> StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public Nullable<System.Guid> DepartmentId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public string DepartmentName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public string DepartmentCode { get; set; }

        //Tên nhân vien
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string SalesEmployeeName { get; set; }

        //Tên nhân vien
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployee_ShortName")]
        public string SalesEmployeeShortName { get; set; }

        public string AbbreviatedName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhoneNumber")]
        public string Phone { get; set; }

        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        public Guid? AccountId { get; set; }

        //Phòng ban
        public string RolesName { get; set; }
    }
}