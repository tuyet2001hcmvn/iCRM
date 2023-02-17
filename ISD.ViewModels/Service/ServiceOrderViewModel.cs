using AutoMapper;
using ISD.Core;
using ISD.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderViewModel
    {
        public string SaleOrg { get; set; }
        public System.Guid ServiceOrderId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderCode")]
        public string ServiceOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderTypeCode")]
        public string ServiceOrderTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_ServiceOrderTypeName")]
        public string ServiceOrderTypeName { get; set; }

        public string ServiceOrderName { get; set; }

        public Nullable<System.Guid> CustomerId { get; set; }
        public Nullable<System.Guid> VehicleId { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_CustomerRequest")]
        [DataType(DataType.MultilineText)]
        public string CustomerRequest { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_KmDaDi")]
        public string KmDaDi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Service_WashRequest")]
        public Nullable<int> WashRequest { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm dd/MM/yyyy}")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string CreatedAccountIdName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1TiepNhan")]
        public Nullable<System.Guid> AccountId1TiepNhan { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1TiepNhanName")]
        public string AccountId1TiepNhanName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step1DateTime")]
        public Nullable<System.DateTime> Step1DateTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step1Note")]
        public string Step1Note { get; set; }


        [Display(Name = "Tư vấn sửa chữa")]
        [DataType(DataType.MultilineText)]
        public string ConsultNote { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat1")]
        public Nullable<System.Guid> AccountId1KyThuat1 { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat1Name")]
        public string AccountId1KyThuat1Name { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat2")]
        public Nullable<System.Guid> AccountId1KyThuat2 { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId1KyThuat2Name")]
        public string AccountId1KyThuat2Name { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId2KiemTra")]
        public Nullable<System.Guid> AccountId2KiemTra { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId2KiemTraName")]
        public string AccountId2KiemTraName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step2DateTime")]
        public Nullable<System.DateTime> Step2DateTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step2HangMuc")]
        public string Step2HangMuc { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step2NextDateTime")]
        public string Step2NextDateTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step2Km")]
        public string Step2Km { get; set; }


        public Nullable<System.Guid> AccountThuNgan { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step")]
        [DataType("Step")]
        public Nullable<int> Step { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhoneNumber")]
        public string CustomerPhone { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string Category { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_ServicePool")]
        public string ServicePool { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_CurrentKm")]
        public Nullable<decimal> CurrentKilometers { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_PersonnalNumberId")]
        public string PersonnalNumberId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        public Nullable<decimal> Total { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Note")]
        public string Note { get; set; }
        public Nullable<bool> isUpdatedFromERP { get; set; }
        public Nullable<System.DateTime> UpdatedFromERPTime { get; set; }
        public string SystemNote { get; set; }
        public Nullable<bool> IsNew { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SystemNote")]
        public int GeneratedCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }
        //public Nullable<System.DateTime> IdentityDate { get; set; }
        //public string IdentityPlace { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Gender")]
        public Nullable<bool> Gender { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_DateOfBirth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProvinceId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> DistrictId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone2 { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_EmailAddress")]
        public string EmailAddress { get; set; }


        public string Fax { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProfitCenterCode")]
        public string ProfitCenterCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProfitCenterCode")]
        public string ProfitCenterName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProductHierarchyCode")]
        public string ProductHierarchyCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProductHierarchyCode")]
        public string ProductHierarchyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string LaborCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureConditionCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ContainerRequirementCode")]
        public string ContainerRequirementCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BuyDate")]
        public Nullable<System.DateTime> BuyDate { get; set; }

        [Display(Name = "Số thứ tự")]
        public int? Number { get; set; }

        public Nullable<System.Guid> Step3AccountId { get; set; }
        public Nullable<System.DateTime> Step3DateTime { get; set; }

        public string AccountId3HoanThanhName { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? Step1DateTimeDuKien { get; set; }
        public string NgayHoanThanhDuKien
        {
            get
            {
                if (Step1DateTimeDuKien.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy}", Step1DateTimeDuKien);
                }
                return string.Empty;
            }
        }
        public string GioHoanThanhDuKien
        {
            get
            {
                if (Step1DateTimeDuKien.HasValue)
                {
                    return string.Format("{0:HH:mm}", Step1DateTimeDuKien);
                }
                return string.Empty;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TotalPrice")]
        public Nullable<decimal> TotalPrice { get; set; }
        
        public int? DiscountType { get; set; }

        [Display(Name = "Chiết khấu")]
        public decimal? Discount { get; set; }

        [Display(Name = "Ghi chú chiết khấu")]
        public string DiscountNote { get; set; }

        [Display(Name = "Lý do hủy")]
        public string DeletedNote { get; set; }

        [Display(Name = "Số tham chiếu với Hãng")]
        public string SoThamChieu { get; set; }

        [Display(Name = "Tổng tiền cọc")]
        public decimal? TongTienCoc { get; set; }
    }
}
