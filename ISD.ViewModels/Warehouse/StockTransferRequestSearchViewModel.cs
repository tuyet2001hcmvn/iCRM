using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockTransferRequestSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Guid? CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid? StoreId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockTransferRequest_FromStock")]
        public Guid? FromStock { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockTransferRequest_ToStock")]
        public Guid? ToStock { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "DocumentDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "DocumentDate")]
        public DateTime? ToDate { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        //public Guid? SearchProductId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockTransferRequestCode")]
        public string StockTransferRequestCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DocumentDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }

        //
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        //public string ProductCode { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        //public string ProductName { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        //public bool? isDelete { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        //public Guid? SearchDepartmentId { get; set; }
    }
}
