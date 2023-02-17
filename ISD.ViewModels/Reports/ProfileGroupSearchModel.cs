using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileGroupSearchModel
    {
        [Display(Name = "Mã nhóm KH")]
        public List<string> ProfileGroupCode { get; set; }
        public bool IsView { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public string CompanyCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonCreateDate")]
        public string CommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

    }
}
