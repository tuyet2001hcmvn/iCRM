using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.EntityModels
{
    [MetadataType(typeof(WarrantyModel.MetaData))]
    public partial class WarrantyModel
    {
        internal sealed class MetaData
        {
            public System.Guid WarrantyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [Remote("CheckExistingWarrantyCode", "Warranty", AdditionalFields = "WarrantyCodeValid, WarrantyCode", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string WarrantyCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string WarrantyName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_Coverage")]
            public string Coverage { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_Duration")]
            public int Duration { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }
        }
    }

    [MetadataType(typeof(ProductWarrantyModel.MetaData))]
    public partial class ProductWarrantyModel
    {
        internal sealed class MetaData
        {
            public System.Guid ProductWarrantyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarrantyCode")]
            public Nullable<int> ProductWarrantyCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_MaterialModel")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid ProductId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_FormDate")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            public System.DateTime FromDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid WarrantyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_SerriNo")]
            [Remote("CheckExistingSerriNo", "ProductWarranty", AdditionalFields = "SerriNoValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
            public string SerriNo { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarrantyNo")]
            public string ProductWarrantyNo { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_ToDate")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> ToDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
            public string SaleOrder { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderDelivery")]
            public string OrderDelivery { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
            public string ERPProductCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
            public string ProfileName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileShortName")]
            public string ProfileShortName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Age")]
            public string Age { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
            public string Phone { get; set; }

            public string Email { get; set; }

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

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
            public Nullable<System.Guid> CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_ActivationQuantity")]
            public Nullable<decimal> ActivatedQuantity { get; set; }
        }
    }
}