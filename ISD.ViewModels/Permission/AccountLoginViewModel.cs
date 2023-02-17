using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISD.EntityModels;

namespace ISD.ViewModels
{
    public class AccountLoginViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyId { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
