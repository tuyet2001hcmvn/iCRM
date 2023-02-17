using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskAccessoryWarrantyReportSearchModel
    {
        //15. Ngày kết thúc
        public string EndCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

        //19. Hình thức bảo hành
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorTypeCode2")]
        public string ErrorTypeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductAccessorySAPCode")]
        public string SearchERPAccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryType")]
        public string SearchAccessoryTypeCode { get; set; }
        //5. Trung tâm bảo hành
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public List<string> ServiceTechnicalTeamCode { get; set; }

        //6. Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> DepartmentCode { get; set; }
        public bool IsView { get; set; }
    }
}