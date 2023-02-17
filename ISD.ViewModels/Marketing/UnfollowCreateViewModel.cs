using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Marketing
{
    public class UnfollowCreateViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company")]
        [Required]
        public string CompanyCode { get; set; }
    }
}
