using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class NumberOfCustomerBySaleGrowthReportViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string SalesEmployeeName { get; set; }
        public decimal? ColNo { get; set; }
        public string ColName { get; set; }
        public decimal? ColValue { get; set; }
    }

    public class NumberOfCustomerBySaleGrowthSearchViewModel
    {
        public string ReportType { get; set; }
        public bool IsView { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CompanyCode { get; set; }
        [Display(Name = "Khách hàng")]
        public List<string> ProfileForeignCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string RolesCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SalesEmployeeCode { get; set; }
        [Display(Name = "Thời gian kì này")]
        public string CurrentDate { get; set; }
        [Display(Name = "Thời gian kì trước")]
        public string PreviousDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? CurrentFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? CurrentToDate { get; set; }  
        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? PreviousFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? PreviousToDate { get; set; }

    }
}
