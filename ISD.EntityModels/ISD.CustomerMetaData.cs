using ISD.Constant;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.EntityModels
{
    [MetadataType(typeof(ProfileModel.MetaData))]
    public partial class ProfileModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileId")]
            public System.Guid ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileCode")]
            public int ProfileCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
            public string ProfileForeignCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsForeignCustomer", Description = "Profile_IsForeignCustomer_Tooltip")]
            public Nullable<bool> isForeignCustomer { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode", Description = "Profile_CustomerTypeCode_Tooltip")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CustomerTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Title")]
            public string Title { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ProfileName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileShortName")]
            public string ProfileShortName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
            //[RegularExpression(@"^(?:0(?:0|1)?)(?:[0-9]{9}|[0-9]{10})$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Phone")]
            //[Remote("CheckExistingPhone", "Profile", AdditionalFields = "PhoneValid, TypeCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string Phone { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Age")]
            public string Age { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
            //[RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Email")]
            [ISDEmailAddress(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Email")]
            [Remote("CheckExistingEmail", "Profile", AdditionalFields = "EmailValid, TypeCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string Email { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
            public string SaleOfficeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
            public string Address { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
            public Nullable<System.Guid> ProvinceId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
            public Nullable<System.Guid> DistrictId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
            public Nullable<System.Guid> WardId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Note")]
            public string Note { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_VisitDate")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> VisitDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ImageUrl")]
            public string ImageUrl { get; set; }

            public string CreateByEmployee { get; set; }
            public string CreateAtCompany { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public string CreateAtSaleOrg { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AbbreviatedName")]
            public string AbbreviatedName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DayOfBirth")]
            public Nullable<int> DayOfBirth { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MonthOfBirth")]
            public Nullable<int> MonthOfBirth { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "YearOfBirth")]
            public Nullable<int> YearOfBirth { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerSourceCode")]
            public string CustomerSourceCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
            public string CustomerGroupCode { get; set; }

            public string Website { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsInvestor")]
            public Nullable<bool> IsInvestor { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsDesigner")]
            public Nullable<bool> IsDesigner { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsContractor")]
            public Nullable<bool> IsContractor { get; set; }

            //Bussiness
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
            [Remote("CheckExistingTaxNo", "Profile", AdditionalFields = "TaxNoValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string TaxNo { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyNumber")]
            public string CompanyNumber { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
            public string CustomerCareerCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_AddressTypeCode")]
            public string AddressTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProjectCode")]
            public int? ProjectCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProjectStatusCode")]
            public string ProjectStatusCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QualificationLevelCode")]
            public string QualificationLevelCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProjectSourceCode")]
            public string ProjectSourceCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
            public Guid? ReferenceProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContractValue")]
            public decimal? ContractValue { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProjectLocation")]
            public string ProjectLocation { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsAnCuongAccessory")]
            public Nullable<bool> IsAnCuongAccessory { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsThiCong")]
            public Nullable<bool> IsThiCong { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Laminate")]
            public string Laminate { get; set; }

            
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_MFC")]
            public string MFC { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Veneer")]
            public string Veneer { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Flooring")]
            public string Flooring { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Accessories")]
            public string Accessories { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_KitchenEquipment")]
            public string KitchenEquipment { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OtherBrand")]
            public string OtherBrand { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_HandoverFurniture")]
            public string HandoverFurniture { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerAccountGroup")]
            public string CustomerAccountGroupCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_isCreateRequest")]
            public bool? isCreateRequest { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_PaymentMethodCode")]
            public string PaymentMethodCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_PartnerFunctionCode")]
            public string PartnerFunctionCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CurrencyCode")]
            public string CurrencyCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxClassificationCode")]
            public string TaxClassificationCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ReconcileAccountCode")]
            public string ReconcileAccountCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_PaymentTermCode")]
            public string PaymentTermCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerAccountAssignmentGroupCode")]
            public string CustomerAccountAssignmentGroupCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Manager")]
            public string Manager { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DebsEmployee")]
            public string DebsEmployee { get; set; }
            //Vốn pháp định
            [DisplayFormat(DataFormatString = "{0:0.#}")]
            public decimal? Number1 { get; set; }

            //Độ phủ thị trường
            [DisplayFormat(DataFormatString = "{0:0.#}")]
            public decimal? Number2 { get; set; }
            [DisplayFormat(DataFormatString = "{0:0.#}")]
            public Nullable<decimal> Number4 { get; set; }


        }
    }

    [MetadataType(typeof(MetaData))]
    public partial class ProfileEmailModel
    {
        internal sealed class MetaData
        {

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Email")]
            //[RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Email")]
            [ISDEmailAddress(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Email")]
            public string Email { get; set; }
        }
    }

    [MetadataType(typeof(ProfileBAttributeModel.MetaData))]
    public partial class ProfileBAttributeModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileId")]
            public System.Guid? ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
            [Remote("CheckExistingTaxNo", "Profile", AdditionalFields = "TaxNoValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string TaxNo { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ContactName")]
            public string ContactName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Position")]
            public string Position { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
            public string CustomerCareerCode { get; set; }
        }
    }

    [MetadataType(typeof(ProfileCAttributeModel.MetaData))]
    public partial class ProfileCAttributeModel
    {
        internal sealed class MetaData
        {
            public System.Guid? ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CompanyId")]
            public Nullable<System.Guid> CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Position")]
            public string Position { get; set; }
        }
    }

    [MetadataType(typeof(AddressBookModel.MetaData))]
    public partial class AddressBookModel
    {
        internal sealed class MetaData
        {
            public System.Guid AddressBookId { get; set; }
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_AddressTypeCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string AddressTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Address")]
            public string Address { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Address2")]
            public string Address2 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
            public Nullable<System.Guid> ProvinceId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
            public Nullable<System.Guid> DistrictId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
            public Nullable<System.Guid> WardId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_Country")]
            public string CountryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Note")]
            public string Note { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_isMain")]
            public Nullable<bool> isMain { get; set; }
        }
    }

    [MetadataType(typeof(PersonInChargeModel.MetaData))]
    public partial class PersonInChargeModel
    {
        internal sealed class MetaData
        {
            public System.Guid PersonInChargeId { get; set; }
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
            public string SalesEmployeeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge_RoleCode")]
            public string RoleCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }
        }
    }

    [MetadataType(typeof(RoleInChargeModel.MetaData))]
    public partial class RoleInChargeModel
    {
        internal sealed class MetaData
        {
            public System.Guid RoleInChargeId { get; set; }
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RoleInCharge")]
            public Nullable<System.Guid> RolesId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }
        }
    }

    [MetadataType(typeof(PartnerModel.MetaData))]
    public partial class PartnerModel
    {
        internal sealed class MetaData
        {
            public System.Guid PartnerId { get; set; }
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerType")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PartnerType { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Partner_PartnerProfileId")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Nullable<System.Guid> PartnerProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }
        }
    }

    [MetadataType(typeof(FileAttachmentModel.MetaData))]
    public partial class FileAttachmentModel
    {
        internal sealed class MetaData
        {
            public System.Guid FileAttachmentId { get; set; }
            public System.Guid ObjectId { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FileAttachmentCode")]
            public string FileAttachmentCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FileAttachmentName")]
            public string FileAttachmentName { get; set; }

            public string FileExtention { get; set; }
            public string FileUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }
        }
    }

    [MetadataType(typeof(ProfileGroupModel.MetaData))]
    public partial class ProfileGroupModel
    {
        internal sealed class MetaData
        {
            public System.Guid ProfileGroupId { get; set; }
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileGroupCode")]
            public string ProfileGroupCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }
        }
    }

    [MetadataType(typeof(ProfilePhoneModel.MetaData))]
    public partial class ProfilePhoneModel
    {
        internal sealed class MetaData
        {
            public System.Guid PhoneId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
            //[RegularExpression(@"^(?:0(?:0|1)?)(?:[0-9]{9}|[0-9]{10})$", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_Phone")]
            [Remote("CheckExistingPhone", "Profile", AdditionalFields = "PhoneValid, TypeCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string PhoneNumber { get; set; }

            public Nullable<System.Guid> ProfileId { get; set; }
        }
    }

    [MetadataType(typeof(ProfileLevelModel.MetaData))]
    public partial class ProfileLevelModel
    {
        internal sealed class MetaData
        {
            public System.Guid CustomerLevelId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerLevelCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CustomerLevelCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerLevelName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string CustomerLevelName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LineOfLevel")]
            public Nullable<decimal> LineOfLevel { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExchangeValue")]
            public Nullable<decimal> ExchangeValue { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectFromDate")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Nullable<System.DateTime> FromDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_EffectToDate")]
            public Nullable<System.DateTime> ToDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Notes")]
            public string Note { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
            public Nullable<Guid> CompanyId { get; set; }
        }
    }

    [MetadataType(typeof(ProfileCategoryModel.MetaData))]
    public partial class ProfileCategoryModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ProfileCategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_ConfigurationName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string ProfileCategoryName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Note")]
            public string Note { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
            public Nullable<System.DateTime> LastEditTime { get; set; }
        }
    }
}