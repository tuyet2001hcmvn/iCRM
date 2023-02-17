using ISD.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProductWarrantyViewModel : ProductWarrantyModel
    {
        public int? STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Profile_ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_MaterialModel")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyName")]
        public string WarrantyName { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string Warranty_ProfileName { get; set; }

        [Display(Name = "Ngày bắt đầu từ")]
        public DateTime? FromDate_From { get; set; }

        [Display(Name = "Ngày bắt đầu đến")]
        public DateTime? FromDate_To { get; set; }

        [Display(Name = "Ngày kết thúc từ")]
        public DateTime? ToDate_From { get; set; }

        [Display(Name = "Ngày kết thúc đến")]
        public DateTime? ToDate_To { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
        public Guid? CompanyId { get; set; }
    }

    public class ProductWarrantySearchViewModel
    {
        public Guid? CompanyId { get; set; }
        public string SerriNo { get; set; }

        public string ProductWarrantyNo { get; set; }

        public Guid? ProfileId { get; set; }

        public string SaleOrder { get; set; }

        public string OrderDelivery { get; set; }

        public string Phone { get; set; }

        public string ProfileName { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }

        public bool? Actived { get; set; }

        public DateTime? FromDate_From { get; set; }
        public DateTime? FromDate_To { get; set; }

        public DateTime? ToDate_From { get; set; }
        public DateTime? ToDate_To { get; set; }
    }
}
