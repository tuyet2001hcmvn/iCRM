using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileGroupReportViewModel
    {
        [Display(Name = "Mã nhóm KH")]
        public string ProfileGroupCode { get; set; }

        [Display(Name = "Nhóm KH")]
        public string ProfileGroupName { get; set; }

        [Display(Name = "Số lượng")]
        public int? NumberOfProfiles { get; set; }

        [Display(Name = "%")]
        public decimal? PercentOfProfiles { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public string CompanyCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonCreateDate")]
        public string CreateDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }
    }
}
