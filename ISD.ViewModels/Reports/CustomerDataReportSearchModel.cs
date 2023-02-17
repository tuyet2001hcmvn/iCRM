using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerDataReportSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_IsForeignCustomer")]
        public Nullable<bool> isForeignCustomer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string CreateAtSaleOrg { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AddressBook_AddressTypeCode")]
        public string AddressTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public string SaleOfficeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public List<Guid> ProvinceId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public Nullable<Guid> DistrictId { get; set; }
        public bool IsView { get; set; }
    }
}
