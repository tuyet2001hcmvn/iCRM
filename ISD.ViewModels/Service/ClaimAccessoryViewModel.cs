using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ClaimAccessoryViewModel
    {
        public Guid ClaimAccessoryId { get; set; }

        public System.Guid ServiceOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public int? Status { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public string StatusName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SendDate")]
        public DateTime? SendDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MeansOfSending")]
        public string MeansOfSending { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SendingSaleOrg")]
        public string SendingSaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Notes")]
        public string AccessoryClaimNote { get; set; }

        #region Thông tin đơn hàng
        //Mã đơn hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderCode")]
        public string ServiceOrderCode { get; set; }

        //Loại đơn hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderTypeCode")]
        public string ServiceOrderTypeCode { get; set; }

        //Loại dịch vụ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderTypeName")]
        public string ServiceOrderTypeName { get; set; }

        //Chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgName { get; set; }

        [Display(Name = "Số thứ tự")]
        public int? Number { get; set; }

        [Display(Name = "Lý do Bảo hành bị từ chối")]
        public string Notes { get; set; }

        [Display(Name = "Số tham chiếu")]
        public string SoThamChieu { get; set; }
        #endregion

        #region Thông tin sửa chữa
        //Yêu cầu của KH
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_CustomerRequest")]
        [DataType(DataType.MultilineText)]
        public string CustomerRequest { get; set; }

        //Tư vấn sửa chữa
        [Display(Name = "Tư vấn sửa chữa")]
        [DataType(DataType.MultilineText)]
        public string ConsultNote { get; set; }

        //Yêu cầu rửa xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_WashRequest")]
        public string WashRequest { get; set; }

        //Ngày yêu cầu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        //Kỹ thuật trưởng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1TiepNhanName")]
        public string AccountId1TiepNhanName { get; set; }
        //Ghi chú cho KTV
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step1Note")]
        public string Step1Note { get; set; }

        //Kỹ thuật viên 1
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat1Name")]
        public string AccountId1KyThuat1Name { get; set; }

        //Kỹ thuật viên 2
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat2Name")]
        public string AccountId1KyThuat2Name { get; set; }

        //Kiểm tra cuối (KTT)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId2KiemTraName")]
        public string AccountId2KiemTraName { get; set; }
        #endregion

        #region Thông tin khách hàng
        public Guid CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_EmailAddress")]
        public string Email { get; set; }

        public string Fax { get; set; }

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

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DateOfIssue")]
        public DateTime? IdentityDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlaceOfIssue")]
        public string IdentityPlace { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }
        #endregion

        #region Thông tin xe
        public Nullable<System.Guid> VehicleId { get; set; }

        //Biển số xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }

        //Số km đã đi
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_KmDaDi")]
        public string KmDaDi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenterName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string ProductHierarchyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string LaborName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureConditionName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BuyDate")]
        public Nullable<System.DateTime> BuyDate { get; set; }
        #endregion

        #region Thông tin phụ tùng/phụ kiện
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Code")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Name")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_FixingType")]
        public string FixingTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public int? Quantity { get; set; }

        [Display(Name = "SL")]
        public int? Quantity_Short { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "ĐVT")]
        public string Unit_Short { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        public decimal? AccessoryPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseCode")]
        public string WarehouseCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockPosition")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Notes")]
        public string DetailNote { get; set; }
        #endregion

        #region Notification
        public DateTime? Status_CreatedTime { get; set; }
        public string Content { get; set; }
        #endregion
    }

    //Tìm kiếm đơn hàng
    public class ClaimAccessorySearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string ServiceOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Code")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Name")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public int? Status { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1TiepNhanName")]
        public string AccountId1TiepNhan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeliveryFromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeliveryToDate")]
        public DateTime? ToDate { get; set; }
    }
}
