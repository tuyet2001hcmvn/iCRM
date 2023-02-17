using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.EntityModels
{
    [MetadataTypeAttribute(typeof(CategoryModel.MetaData))]
    public partial class CategoryModel
    {
        internal sealed class MetaData
        {
            public Guid CategoryId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Brand_BrandCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingCategoryCode", "Brand", AdditionalFields = "CategoryCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string CategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Brand_BrandName")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CategoryName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
            public string ImageUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category_TrackTrend")]
            public Nullable<bool> IsTrackTrend { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ConfigurationModel.MetaData))]
    public partial class ConfigurationModel
    {
        internal sealed class MetaData
        {
            public System.Guid ConfigurationId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingConfigurationCode", "Configuration", AdditionalFields = "ConfigurationCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string ConfigurationCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ConfigurationName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(StyleModel.MetaData))]
    public partial class StyleModel
    {
        internal sealed class MetaData
        {
            public System.Guid StyleId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Style_StyleCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingStyleCode", "Style", AdditionalFields = "StyleCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string StyleCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Style_StyleName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string StyleName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ColorModel.MetaData))]
    public partial class ColorModel
    {
        internal sealed class MetaData
        {
            public System.Guid ColorId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ColorCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorShortName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingColorShortName", "Color", AdditionalFields = "ColorShortNameValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string ColorShortName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ColorName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(SpecificationsModel.MetaData))]
    public partial class SpecificationsModel
    {
        internal sealed class MetaData
        {
            public System.Guid SpecificationsId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Specifications_SpecificationsCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingSpecificationsCode", "Specifications", AdditionalFields = "SpecificationsCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string SpecificationsCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Specifications_SpecificationsName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string SpecificationsName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(AccessoryCategoryModel.MetaData))]
    public partial class AccessoryCategoryModel
    {
        internal sealed class MetaData
        {
            public System.Guid AccessoryCategoryId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategory_AccessoryCategoryCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string AccessoryCategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategory_AccessoryCategoryName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string AccessoryCategoryName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(AccessoryModel.MetaData))]
    public partial class AccessoryModel
    {
        internal sealed class MetaData
        {
            public System.Guid AccessoryId { get; set; }

            public System.Guid AccessoryCategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingAccessoryCode", "Accessory", AdditionalFields = "AccessoryCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string AccessoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string AccessoryName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryType")]
            public string AccessoryType { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
            public string AccessoryUnit { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
            public string ImageUrl { get; set; }
        }
    }
    [MetadataTypeAttribute(typeof(PeriodicallyCheckingModel.MetaData))]
    public partial class PeriodicallyCheckingModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PeriodicallyCheckingCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PeriodicallyCheckingCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PeriodicallyCheckingName")]
            public string PeriodicallyCheckingName { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
            public string Description { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Image")]
            public string FileUrl { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
            public Nullable<System.DateTime> CreatedDate { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedUser")]
            public string CreatedUser { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastModifyDate")]
            public Nullable<System.DateTime> LastModifyDate { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastModifyUser")]
            public string LastModifyUser { get; set; }
        }
    }
    [MetadataTypeAttribute(typeof(PlateFeeModel.MetaData))]
    public partial class PlateFeeModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PlateFeeCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PlateFeeName")]
            public string PlateFeeName { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
            public string Description { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedUser { get; set; }
            public Nullable<System.DateTime> LastModifyDate { get; set; }
            public string LastModifyUser { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(WarehouseModel.MetaData))]
    public partial class WarehouseModel
    {
        internal sealed class MetaData
        {
            public System.Guid WarehouseId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid StoreId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingWarehouseCode", "Warehouse", AdditionalFields = "WarehouseCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string WarehouseCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string WarehouseName { get; set; }

            
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseShortName")]
            [StringLength(33, ErrorMessage = "Vui lòng nhập \"{0}\" tối đa 33 ký tự.")]
            public string WarehouseShortName { get; set; }
            

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(MaterialGroupModel.MetaData))]
    public partial class MaterialGroupModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_MaterialGroupCode")]
            public string MaterialGroupCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_MaterialGroupName")]
            public string MaterialGroupName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_IconUrl")]
            public string IconUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceFee")]
            public decimal? ServiceFee { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ProfitCenterModel.MetaData))]
    public partial class ProfitCenterModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfitCenter_ProfitCenterCode")]
            public string ProfitCenterCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfitCenter_ProfitCenterName")]
            public string ProfitCenterName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ProductHierarchyModel.MetaData))]
    public partial class ProductHierarchyModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductHierarchy_ProductHierarchyCode")]
            public string ProductHierarchyCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductHierarchy_ProductHierarchyName")]
            public string ProductHierarchyName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
            public string ImageUrl { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(LaborModel.MetaData))]
    public partial class LaborModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Labor_LaborCode")]
            public string LaborCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Labor_LaborName")]
            public string LaborName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(MaterialFreightGroupModel.MetaData))]
    public partial class MaterialFreightGroupModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialFreightGroup_MaterialFreightGroupCode")]
            public string MaterialFreightGroupCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialFreightGroup_MaterialFreightGroupName")]
            public string MaterialFreightGroupName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ExternalMaterialGroupModel.MetaData))]
    public partial class ExternalMaterialGroupModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExternalMaterialGroup_ExternalMaterialGroupCode")]
            public string ExternalMaterialGroupCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExternalMaterialGroup_ExternalMaterialGroupName")]
            public string ExternalMaterialGroupName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(TemperatureConditionModel.MetaData))]
    public partial class TemperatureConditionModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TemperatureCondition_TemperatureConditionCode")]
            public string TemperatureConditionCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TemperatureCondition_TemperatureConditionName")]
            public string TemperatureConditionName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(ContainerRequirementModel.MetaData))]
    public partial class ContainerRequirementModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContainerRequirement_ContainerRequirementCode")]
            public string ContainerRequirementCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContainerRequirement_ContainerRequirementName")]
            public string ContainerRequirementName { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(AccessorySaleOrderModel.MetaData))]
    public partial class AccessorySaleOrderModel
    {
        internal sealed class MetaData
        {
            [Display(Name = "Khách hàng")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Guid? CustomerId { get; set; }
        }
    }
}
