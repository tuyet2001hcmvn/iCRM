using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Service
{
    public class FindVehicleViewModel
    {
        public Guid VehicleId { get; set; }

        #region Thông tin khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Guid? CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhoneNumber")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public Nullable<System.Guid> ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public Nullable<System.Guid> DistrictId { get; set; }
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public Nullable<System.Guid> WardId { get; set; }
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }
        #endregion

        #region Thông tin xe
        //Số khung
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        //Số máy
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        //Biển số xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }

        //Số km hiện tại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_CurrentKm")]
        public decimal? CurrentKilometers { get; set; }

        //Số sổ BH
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyNumber")]
        public string WarrantyNumber { get; set; }

        //Chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrg { get; set; }

        //Công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
        public Guid? CompanyId { get; set; }

        public string CompanyName { get; set; }

        //Nhãn hiệu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenterCode { get; set; }

        //Nhãn hiệu name
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenterName { get; set; }

        //Loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string ProductHierarchyCode { get; set; }

        //Loại xe name
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string ProductHierarchyName { get; set; }

        //Số bình điện
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBinhDien")]
        public string SoBinhDien { get; set; }

        #endregion

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BuyDate")]
        public DateTime? BuyDate { get; set; }

        public List<ServiceHistoryViewModel> HistoryList { get; set; }
    }
}
