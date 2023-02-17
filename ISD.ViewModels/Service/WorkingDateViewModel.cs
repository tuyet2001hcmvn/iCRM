using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class WorkingDateViewModel
    {
        #region View Model cũ
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_FromDate")]
        //public DateTime? FromDate { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_ToDate")]
        //public DateTime? ToDate { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        //public bool DayOff { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_DayOfWeek")]
        //public List<int> DayOfWeek { get; set; }

        //public int? DayOfWeekIndex { get; set; }

        //public string DayOfWeekName { get; set; }

        ////Time
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_FromTime")]
        //public TimeSpan? FromTime { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_ToTime")]
        //public TimeSpan? ToTime { get; set; }

        ////Time Config
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_FromTime")]
        //public TimeSpan? WorkingFromTime { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_ToTime")]
        //public TimeSpan? WorkingToTime { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_Interval", Description = "WorkingTime_Interval_Hint")]
        //public int? Interval { get; set; }

        ////TimeFrame
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_DayOfWeek")]
        //public int DayOfWeekTimeFrame { get; set; }

        //public List<TimeFrameList> TimeFrame { get; set; }

        //public class TimeFrameList
        //{
        //    [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_FromTime")]
        //    public TimeSpan? TimeFrameFrom { get; set; }

        //    [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_ToTime")]
        //    public TimeSpan? TimeFrameTo { get; set; }

        //    [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingTime_Amount")]
        //    public int? Amount { get; set; }
        //}

        ////Company
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        //public Guid? CompanyId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        //public Guid? WorkingTimeCompanyId { get; set; }

        ////Store
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        //public Guid StoreId { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        //public Guid WorkingTimeStoreId { get; set; }

        //public bool Actived { get; set; }
        #endregion

        public Guid AppointmentId { get; set; }

        public Guid? Common_AppointmentId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceType")]
        public string ServiceTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AppointmentDate")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? AppointmentDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AppointmentTime")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public TimeSpan? AppointmentTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Notes")]
        public string Notes { get; set; }

        //Search ViewModel
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public Guid? CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkingDate_ToDate")]
        public DateTime? ToDate { get; set; }

        //Modal
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string Modal_SaleOrgCode { get; set; }
    }
}
