using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class DeliveryViewModel
    {
        public System.Guid DeliveryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeliveryCode")]
        public int DeliveryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DocumentDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Nullable<System.Guid> CompanyId { get; set; }

        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Nullable<System.Guid> StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SalesEmployeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public string LastEditByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public Nullable<System.DateTime> LastEditTime { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> isDeleted { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientName")]
        public string RecipientName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientAddress")]
        public string RecipientAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientPhone")]
        public string RecipientPhone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientCompany")]
        public string RecipientCompany { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderAddress")]
        public string SenderAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderPhone")]
        public string SenderPhone { get; set; }

        public Guid? TaskId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Showroom_GTB")]
        public string Summary { get; set; }

        public bool? IsEdit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CatalogueValueTotal")]
        public decimal? Total { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeletedReason")]
        public string DeletedReason { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ShippingTypeCode")]
        [Required]
        public string ShippingTypeCode { get; set; }

        public int? STT { get; set; }

        public string DeliveryType { get; set; }
    }
}