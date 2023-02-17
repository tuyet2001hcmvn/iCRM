using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class StockReceivingMasterViewModel
    {
        public System.Guid StockReceivingId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceivingCode")]
        public int StockReceivingCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        public Nullable<System.DateTime> DocumentDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> CompanyId { get; set; }

        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> StoreId { get; set; }

        public string StoreCode { get; set; }
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SalesEmployeeCode { get; set; }

        public string SalesEmployeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProfileId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProfileId { get; set; }

        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        public bool? isFirst { get; set; }

        public bool? isDeleted { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeletedReason")]
        public string DeletedReason { get; set; }
    }
}