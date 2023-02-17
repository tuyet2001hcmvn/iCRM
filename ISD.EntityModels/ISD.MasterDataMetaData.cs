using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.EntityModels
{
    [MetadataTypeAttribute(typeof(CompanyModel.MetaData))]
    public partial class CompanyModel
    {
        internal sealed class MetaData
        {
            public Guid CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_CompanyCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingCompanyCode", "Company", AdditionalFields = "CompanyCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string CompanyCode { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string Plant { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_CompanyName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CompanyName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_TelProduct")]
            public string TelProduct { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_TelService")]
            public string TelService { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_CompanyAddress")]
            public string CompanyAddress { get; set; }

            public string Logo { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
            public string TaxCode { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(StoreModel.MetaData))]
    public partial class StoreModel
    {
        internal sealed class MetaData
        {
            public System.Guid StoreId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
            public Guid CompanyId { get; set; }

            //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            //[Remote("CheckExistingStoreCode", "Store", AdditionalFields = "StoreCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            //public string StoreCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string SaleOrgCode { get; set; }

            //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_SaleOrgCode_KetToan")]
            //public string SaleOrgCode_KetToan { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string StoreName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InvoiceStoreName")]
            public string InvoiceStoreName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_TelProduct")]
            public string TelProduct { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_TelService")]
            public string TelService { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreAddress")]
            public string StoreAddress { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(PromotionModel.MetaData))]
    public partial class PromotionModel
    {
        internal sealed class MetaData
        {
            public Guid PromotionId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingPromotionCode", "Promotion", AdditionalFields = "PromotionCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string PromotionCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PromotionName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionContent")]
            public string PromotionContent { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public string SaleOrgCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
            public DateTime? FromDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
            public DateTime? ToDate { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(CustomerPromotionModel.MetaData))]
    public partial class CustomerPromotionModel
    {
        internal sealed class MetaData
        {
            public System.Guid PromotionId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingPromotionCode", "CustomerPromotion", AdditionalFields = "PromotionCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string PromotionCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PromotionName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
            public Nullable<System.DateTime> EffectFromDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
            public Nullable<System.DateTime> EffectToDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Description", Description = "Promotion_Description_Hint")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Notes")]
            public string Notes { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(CustomerLevelModel.MetaData))]
    public partial class CustomerLevelModel
    {
        internal sealed class MetaData
        {
            public System.Guid CustomerLevelId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerLevel_CustomerLevelCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingCustomerLevelCode", "CustomerLevel", AdditionalFields = "CustomerLevelCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string CustomerLevelCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerLevel_CustomerLevelName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CustomerLevelName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(CustomerGiftModel.MetaData))]
    public partial class CustomerGiftModel
    {
        internal sealed class MetaData
        {
            public System.Guid GiftId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingGiftCode", "CustomerGift", AdditionalFields = "GiftCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string GiftCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_PromotionName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string GiftName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
            public Nullable<System.DateTime> EffectFromDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
            public Nullable<System.DateTime> EffectToDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Description", Description = "Promotion_Description_Hint")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ImageUrl", Description = "Promotion_ImageUrl_Hint")]
            public string ImageUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_Notes")]
            public string Notes { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ProvinceModel.MetaData))]
    public partial class ProvinceModel
    {
        internal sealed class MetaData
        {
            public System.Guid ProvinceId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_ProvinceCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingProvinceCode", "Province", AdditionalFields = "ProvinceCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string ProvinceCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_ProvinceName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ProvinceName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Province_Area")]
            public string Area { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(CareerModel.MetaData))]
    public partial class CareerModel
    {
        internal sealed class MetaData
        {
            public System.Guid CareerId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Career_CareerCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingCareerCode", "Career", AdditionalFields = "CareerCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string CareerCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Career_CareerName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CareerName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ServiceRequirementModel.MetaData))]
    public partial class ServiceRequirementModel
    {
        internal sealed class MetaData
        {
            public System.Guid ServiceRequirementId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceRequirement_ServiceRequirementCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingServiceRequirementCode", "ServiceRequirement", AdditionalFields = "ServiceRequirementCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string ServiceRequirementCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceRequirement_ServiceRequirementName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ServiceRequirementName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(StoreTypeModel.MetaData))]
    public partial class StoreTypeModel
    {
        internal sealed class MetaData
        {
            public Guid StoreTypeId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StoreType_StoreTypeName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string StoreTypeName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(SalesEmployeeModel.MetaData))]
    public partial class SalesEmployeeModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployee_Code")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingSalesEmployeeCode", "SalesEmployee", AdditionalFields = "SalesEmployeeCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string SalesEmployeeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyId")]
            public Nullable<System.Guid> CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public Nullable<System.Guid> StoreId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
            public Nullable<System.Guid> DepartmentId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployee_Name")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string SalesEmployeeName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployee_ShortName")]
            public string SalesEmployeeShortName { get; set; }

            public string AbbreviatedName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
            public string Email { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhoneNumber")]
            public string Phone { get; set; }

            public Nullable<System.Guid> CreateBy { get; set; }
            public Nullable<System.DateTime> CreateTime { get; set; }
            public Nullable<System.Guid> LastEditBy { get; set; }
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataType(typeof(CatalogTypeModel.MetaData))]
    public partial class CatalogTypeModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CatalogTypeCode")]
            public string CatalogTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CatalogTypeName")]
            public string CatalogTypeName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataType(typeof(CatalogModel.MetaData))]
    public partial class CatalogModel
    {
        internal sealed class MetaData
        {
            public System.Guid CatalogId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CatalogTypeCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CatalogTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalog_CatalogCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CatalogCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalog_CatalogText_en")]
            public string CatalogText_en { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalog_CatalogText_vi")]
            public string CatalogText_vi { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataType(typeof(DepartmentModel.MetaData))]
    public partial class DepartmentModel
    {
        internal sealed class MetaData
        {
            public System.Guid DepartmentId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
            public Nullable<System.Guid> CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
            public Nullable<System.Guid> StoreId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingDepartmentCode", "Department", AdditionalFields = "DepartmentCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string DepartmentCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentName")]
            public string DepartmentName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            public Nullable<System.Guid> CreateBy { get; set; }
            public Nullable<System.DateTime> CreateTime { get; set; }
            public Nullable<System.Guid> LastEditBy { get; set; }
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataType(typeof(ProductModel.MetaData))]
    public partial class ProductModel
    {
        internal sealed class MetaData
        {
            public System.Guid ProductId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductCode")]
            public string ProductCode { get; set; }

            public string ERPProductCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductName")]
            public string ProductName { get; set; }

            public Nullable<System.Guid> BrandId { get; set; }
            public Nullable<decimal> CylinderCapacity { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Category")]
            public Nullable<System.Guid> CategoryId { get; set; }

            public System.Guid ConfigurationId { get; set; }
            public string GuaranteePeriod { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
            public string ImageUrl { get; set; }

            public Nullable<bool> isHot { get; set; }
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public bool Actived { get; set; }

            public Nullable<System.Guid> CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Inventory")]
            public Nullable<bool> isInventory { get; set; }
        }
    }

    [MetadataType(typeof(KanbanModel.MetaData))]
    public partial class KanbanModel
    {
        internal sealed class MetaData
        {
            public System.Guid KanbanId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Kanban_KanbanCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingKanbanCode", "Kanban", AdditionalFields = "KanbanCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string KanbanCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Kanban_KanbanName")]
            public string KanbanName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }
            public Nullable<System.Guid> LastEditBy { get; set; }
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            public Nullable<bool> OrderIndex { get; set; }
        }
    }

    [MetadataType(typeof(KanbanDetailModel.MetaData))]
    public partial class KanbanDetailModel
    {
        internal sealed class MetaData
        {
            public Guid KanbanDetailId { get; set; }
            public System.Guid? KanbanId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Kanban_ColumnName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ColumnName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }

            public Nullable<System.Guid> CreateBy { get; set; }
            public Nullable<System.DateTime> CreateTime { get; set; }
            public Nullable<System.Guid> LastEditBy { get; set; }
            public Nullable<System.DateTime> LastEditTime { get; set; }
        }
    }

    [MetadataType(typeof(NewsModel.MetaData))]
    public partial class NewsModel
    {
        internal sealed class MetaData
        {
            public System.Guid NewsId { get; set; }

            public int NewsCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_NewsCategoryId")]
            public System.Guid NewsCategoryId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Title")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string Title { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_ScheduleTime")]
            public DateTime? ScheduleTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_ImageUrl")]
            public string ImageUrl { get; set; }
            
            public Nullable<bool> isShowOnMobile { get; set; }
            
            public Nullable<bool> isShowOnWeb { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_CreateBy")]
            public System.Guid CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_CreateTime")]
            public DateTime? CreateTime { get; set; }
            
            public System.Guid LastEditBy { get; set; }

            public DateTime? LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_Detail")]
            public string Detail { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_Summary")]
            public string Summary { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_TypeNews")]
            public string TypeNews { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_EndTime")]
            public Nullable<System.DateTime> EndTime { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_GroupEmployee")]
            public Nullable<System.Guid> GroupEmployeeId { get; set; }
        }
    }

    [MetadataType(typeof(NewsCategoryModel.MetaData))]
    public partial class NewsCategoryModel
    {
        internal sealed class MetaData
        {
            public System.Guid NewsCategoryId { get; set; }

            public int NewsCategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewsCategory_NewsCategoryName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string NewsCategoryName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewsCategory_Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "News_ImageUrl")]
            public string ImageUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewsCategory_OrderIndex")]
            public int? OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewsCategory_CreateBy")]
            public System.Guid CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewsCategory_CreateTime")]
            public DateTime? CreateTime { get; set; }

            public System.Guid LastEditBy { get; set; }

            public DateTime? LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }
}