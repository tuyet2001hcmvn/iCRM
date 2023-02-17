using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels
{
    public class UnfollowCreateViewModel
    {
        [Required]
        public string Email { get; set; }
        public string CompannyCode { get; set; }
    }
}
