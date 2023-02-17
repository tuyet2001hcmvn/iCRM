using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class CustomerSearchViewModel
    {
        public Guid? CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerLoyaltyCard")]
        public string SearchCustomerLoyaltyCard { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        public Guid? SearchCustomerLevelId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string SearchFullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string SearchPhone { get; set; }
    }
}
