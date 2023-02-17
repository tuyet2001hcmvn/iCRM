using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AppointmentSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
        public Guid? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> SaleOfficeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StoreId")]
        public List<Guid>  StoreId { get; set; }
        public string SaleOrgCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskCode")]
        public string TaskCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Summary")]
        public string Summary { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerSourceCode")]
        public List<string> CustomerSourceCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_SaleEmployeeCode")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Age")]
        public string Age { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerClass")]
        public string CustomerClassCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus", Description = "StatusOfTask")]
        public Guid? TaskStatusId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
        public string CustomerCareerCode { get; set; }
        //Ngày tiếp nhận
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonReceiveDate")]
        public string CommonReceiveDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_ReceiveDate")]
        public DateTime? ReceiveFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_ReceiveDate")]
        public DateTime? ReceiveToDate { get; set; }
        //Ngày kết thúc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonEndDate")]
        public string CommonEndDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_EndDate")]
        public DateTime? EndFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_EndDate")]
        public DateTime? EndToDate { get; set; }
        //Ngày tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonCreateDate")]
        public string CommonCreateDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_CreateFromDate")]
        public DateTime? CreateFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_CreateToDate")]
        public DateTime? CreateToDate { get; set; }

        //Mobile additional field
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
        public List<string> CustomerGroupCode { get; set; }
        public bool? isViewByStore { get; set; }

        public string CustomerCategoryCode { get; set; }
        public Guid? CategoryId { get; set; }

        //Khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        public int ProfileCode { get; set; }

        public int? NumberIndex { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool IsView { get; set; }
    }
}
