using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class StockReceivingSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Guid? SearchCompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid? SearchStoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        public List<Guid> SearchStockId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SearchSalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProfileId")]
        public Guid? SearchProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "DocumentDate")]
        public DateTime? SearchFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "DocumentDate")]
        public DateTime? SearchToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public List<Guid> SearchProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceivingCode")]
        public string SearchStockReceivingCode { get; set; }

        //
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceiving_ProfileCode")]
        public string ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceiving_ProfileName")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCodeSearch")]
        public string ProductCodeSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? isDelete { get; set; }
        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public Nullable<System.Guid> DepartmentId { get; set; }

        //Nhóm khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileGroup")]
        public string SearchProfileGroupCode { get; set; }
        public bool IsView { get; set; }
    }
}