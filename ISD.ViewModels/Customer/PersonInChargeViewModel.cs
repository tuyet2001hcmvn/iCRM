using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class PersonInChargeViewModel : BaseClassViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_EmpCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_RoleCode")]
        public string RoleCode { get; set; }
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeShortName { get; set; }

        public System.Guid PersonInChargeId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }
        public string CompanyCode { get; set; }
        public int? SalesEmployeeType { get; set; }
    }

    public class PersonInChargeNotRequiredViewModel : BaseClassViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_EmpCode")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_RoleCode")]
        public string RoleCode { get; set; }
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeShortName { get; set; }
        public System.Guid PersonInChargeId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }
        public string CompanyCode { get; set; }
        public int? SalesEmployeeType { get; set; }
    }
}