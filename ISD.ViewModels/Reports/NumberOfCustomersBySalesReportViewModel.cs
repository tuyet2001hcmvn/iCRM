using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class NumberOfCustomersBySalesReportViewModel
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

    public class NumberOfCustomersBySalesSearchViewModel
    {
        public string ReportType { get; set; }
        public bool IsView { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CompanyCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string RolesCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime ToDate { get; set; }

    }
}
