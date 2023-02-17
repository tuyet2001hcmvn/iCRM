using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class LoginMobileResultViewModel
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public bool? Actived { get; set; }
        public bool? isViewByStore { get; set; }
        public bool? isCreatePrivateTask { get; set; }

        public List<string> RolesCodeLst { get; set; }
        public bool? isSRNQ { get; set; }
        //Default value
        public string DefaultSaleOfficeCode { get; set; }
        public string DefaultCustomerSourceCode { get; set; }
        public string DefaultCreateAtSaleOrg { get; set; }
        public string GoogleMapAPIKey { get; set; }
    }
}
