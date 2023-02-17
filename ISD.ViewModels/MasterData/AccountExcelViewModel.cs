using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class AccountExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Excel_AccountId")]
        public Guid AccountId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        [Required]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        [Required]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Account_FullName")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployee_Code")]
        public string EmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Account_ShowChoseModule")]
        public bool? isShowChoseModule { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Account_ViewTotal")]
        public bool? isShowDashboard { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_RolesModel")]
        [Required]
        public string RoleList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AcountExcel_SalesOrg")]
        public string StoreList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
