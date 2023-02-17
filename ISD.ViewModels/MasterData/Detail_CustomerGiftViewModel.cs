using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class Detail_CustomerGiftViewModel
    {
        //Customer
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Guid CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        public Guid? CustomerLevelId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_CustomerLevel")]
        public string CustomerLevelName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerLoyaltyCard")]
        public string CustomerLoyaltyCard { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CumulativePoint")]
        public decimal? CumulativePoint { get; set; }
    }
}
