using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class DepartmentViewModel
    {
        public System.Guid DepartmentId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public Nullable<System.Guid> CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingDepartmentCode", "Department", AdditionalFields = "DepartmentCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string DepartmentCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentName")]
        public string DepartmentName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}