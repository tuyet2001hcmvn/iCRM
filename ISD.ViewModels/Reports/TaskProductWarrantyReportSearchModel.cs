using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskProductWarrantyReportSearchModel
    {
        //15. Ngày kết thúc
        public string EndCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductSAPCode")]
        public string SearchERPProductCode { get; set; }

        [Display(Name = "Phân loại sản phẩm")]
        public string ProductCategoryCode { get; set; }

        //20. Phương thức xử lý
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorCode2")]
        public string ErrorCode { get; set; }

        //4. Loại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public List<Guid> WorkFlowId { get; set; }
        //5. Trung tâm bảo hành
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public List<string> ServiceTechnicalTeamCode { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> DepartmentCode { get; set; }
        public bool IsView { get; set; }
    }
}
