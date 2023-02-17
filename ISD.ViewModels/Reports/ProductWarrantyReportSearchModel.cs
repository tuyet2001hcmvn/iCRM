using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductWarrantyReportSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
        public Guid? CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public List<string> ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileName")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhoneNumber")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public List<Guid> ProvinceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public List<Guid> DistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public List<Guid> WardId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductSAPCode")]
        public List<string> ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductName")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SerialNumber")]
        public List<string> SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        public List<string> SaleOrder { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderDelivery")]
        public List<string> OrderDelivery { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyCode")]
        public List<string> WarrantyCode { get; set; }

        //15. Ngày Bắt đầu
        public string FromCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndFromDate { get; set; }
        //15. Ngày kết thúc
        public string ToCommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

     
        public bool IsView { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}
