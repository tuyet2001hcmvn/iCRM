using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class DeliverySearchViewModel
    {
        //Mã phiếu xuất
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeliveryCode")]
        public int? SearchDeliveryCode { get; set; }

        //ngày chứng từ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "DocumentDate")]
        public DateTime? SearchFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "DocumentDate")]
        public DateTime? SearchToDate { get; set; }

        //Công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Nullable<System.Guid> SearchCompanyId { get; set; }

        //chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Nullable<System.Guid> SearchStoreId { get; set; }

        //Kho xuất
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_StockExport")]
        public List<Nullable<System.Guid>> SearchStockId { get; set; }

        //Phòng ban 
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public Nullable<System.Guid> DepartmentId { get; set; }

        //Nhân viên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SearchSalesEmployeeCode { get; set; }

        //khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Nullable<System.Guid> SearchProfileId { get; set; }

        //Nhóm khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileGroup")]
        public string SearchProfileGroupCode { get; set; }

        //Loại Catalogue
        [Display(Name = "Loại Catalogue")]
        public Nullable<System.Guid> SearchCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string SearchProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string SearchProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product")]
        public List<Guid?> SearchProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> isDeleted { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCodeSearch")]
        public string ProductCodeSearch { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool IsView { get; set; }
    }
}