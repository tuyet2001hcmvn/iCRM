using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ISD.Resources;
namespace ISD.EntityModels
{
    [MetadataTypeAttribute(typeof(RolesModel.MetaData))]
    public partial class RolesModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Export_ExcelCode")]
            public Guid RolesId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingRolesCode", "Roles", AdditionalFields = "RolesCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string RolesCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string RolesName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles_isCheckLoginByTime")]
            public Nullable<bool> isCheckLoginByTime { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isEmployeeGroup")]
            public Nullable<bool> isEmployeeGroup { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isSendSMSPermission")]
            public Nullable<bool> isSendSMSPermission { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles_WorkingTimeFrom")]
            [DisplayFormat(DataFormatString = "{0:s}", ApplyFormatInEditMode = true)]
            public Nullable<DateTime> WorkingTimeFrom { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles_WorkingTimeTo")]
            [DisplayFormat(DataFormatString = "{0:s}", ApplyFormatInEditMode = true)]
            public Nullable<DateTime> WorkingTimeTo { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(FunctionModel.MetaData))]
    public partial class FunctionModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FunctionId")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string FunctionId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FunctionName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string FunctionName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            public Nullable<int> OrderIndex { get; set; }

        }
    }

    [MetadataTypeAttribute(typeof(ModuleModel.MetaData))]
    public partial class ModuleModel
    {
        internal sealed class MetaData
        {
            public System.Guid ModuleId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ModuleName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ModuleName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            public Nullable<int> Icon { get; set; }
        }
    }


    [MetadataTypeAttribute(typeof(MenuModel.MetaData))]
    public partial class MenuModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuId")]
            public System.Guid MenuId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ModuleName")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Nullable<System.Guid> ModuleId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string MenuName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            public Nullable<int> Icon { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(PageModel.MetaData))]
    public partial class PageModel
    {
        internal sealed class MetaData
        {
            public System.Guid PageId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PageName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PageName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PageUrl")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PageUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_MenuModel")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid MenuId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            public string Icon { get; set; }
            public string Color { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(AccountModel.MetaData))]
    public partial class AccountModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingUserName", "Account", AdditionalFields = "UserNameValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string UserName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
            public string FullName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesEmployeeCode_SAP")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingEmployeeCode", "Account", AdditionalFields = "EmployeeCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string EmployeeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isShowChoseModule")]
            public Nullable<bool> isShowChoseModule { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isViewByStore")]
            public Nullable<bool> isViewByStore { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isViewTotal")]
            public Nullable<bool> isViewTotal { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isViewByWarehouse")]
            public Nullable<bool> isViewByWarehouse { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isReceiveNotification")]
            public Nullable<bool> isReceiveNotification { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_RolesModel")]
            public ICollection<RolesModel> RolesModel { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public ICollection<StoreModel> StoreModel { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskFilterCode")]
            public string TaskFilterCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isCreatePrivateTask")]
            public Nullable<bool> isCreatePrivateTask { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isViewRevenue")]
            public bool? isViewRevenue { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Project_ViewPermission")]
            public string ViewPermission { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isViewOpportunity")]
            public Nullable<bool> isViewOpportunity { get; set; }
        }
    }


    [MetadataTypeAttribute(typeof(MobileScreenModel.MetaData))]
    public partial class MobileScreenModel
    {
        internal sealed class MetaData
        {
            public System.Guid MobileScreenId { get; set; }
            //Tên màn hình
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScreenName")]
            public string ScreenName { get; set; }
            //Đường dẫn
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScreenCode")]
            public string ScreenCode { get; set; }

            public Nullable<System.Guid> MenuId { get; set; }
            //Loại icon
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IconType")]
            public string IconType { get; set; }
            //Tên icon
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IconName")]
            public string IconName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderBy")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            public string Icon { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Mobile_Visible")]
            public Nullable<bool> Visible { get; set; }

            public Nullable<bool> isSystem { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isCreated")]
            public Nullable<bool> isCreated { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isReporter")]
            public Nullable<bool> isReporter { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isAssignee")]
            public Nullable<bool> isAssignee { get; set; }

            //public virtual ICollection<MobileScreenPermissionModel> MobileScreenPermissionModel { get; set; }
            //public virtual ICollection<FunctionModel> FunctionModel { get; set; }
        }
    }
}
