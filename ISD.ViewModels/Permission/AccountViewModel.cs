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
    public class AccountViewModel : AccountModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "retypePassword")]
        [NotMapped]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "",
                                      ErrorMessageResourceName = "CheckretypePassword",
                                      ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string retypePassword { get; set; }

        public List<RolesModel> RolesList { get; set; }

        public List<RolesModel> ActivedRolesList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_RolesModel")]
        public Guid RolesId { get; set; }

        //Permission Company
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        public List<CompanyModel> ActivedCompanyList { get; set; }

        //Permission Store
        public List<StoreModel> StoreList { get; set; }

        public List<StoreModel> ActivedStoreList { get; set; }

        public Guid StoreId { get; set; }

        public string StoreName { get; set; }
    }
}
