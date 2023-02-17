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
    public class LoginMobileViewModel
    {
        public Guid AccountId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string EmployeeCode { get; set; }

        public bool? Actived { get; set; }

        public List<string> RoleCodeList { get; set; }

        //public List<string>

        public List<MobileScreenViewModel> MobileScreen { get; set; }
        public List<MobileScreenPermissionViewModel> MobileScreenPermission { get; set; }
        public List<string> CatalogPermission { get; set; }
        public List<ApplicationConfig> AppConfig { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public List<ISDSelectStringItem> StoreCodeList { get; set; }
        public List<ISDSelectStringItem> WarehouseList { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        //Menu phân quyền theo nhóm người dùng
        public string DrawerToLoad { get; set; }
    }
}
