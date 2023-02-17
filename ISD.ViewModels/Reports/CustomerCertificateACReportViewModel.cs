using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerCertificateACReportViewModel
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberIndex")]
        public int NumberIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public int? ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_Content")]
        public string Content { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_StartDate")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_EndDate")]
        public DateTime? EndDate { get; set; }

    }
    public class CustomerCertificateACReportSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public List<string> SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_Content")]
        public string Content { get; set; }


        //Ngày hiệu lực
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }

        //Ngày đến hạn
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string EndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }
        public bool IsView { get; set; }

        public string ReportType { get; set; }
    }
}
