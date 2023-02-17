using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionDetailViewModel
    {
        public int? STT { get; set; }
        [Display(Name = "Chi tiết")]
        public System.Guid ProductPromotionDetailId { get; set; }
        [Display(Name = "Liên hệ")]
        public System.Guid ProductPromotionContactId { get; set; }
        [Display(Name = "Quảng bá sản phẩm")]
        public Nullable<System.Guid> ProductPromotionId { get; set; }
        [Display(Name = "Khách hàng")]
        public Nullable<System.Guid> ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public int? ProfileCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileName")]
        public string ProfileName { get; set; }
        [Display(Name = "Tên ngắn")]
        public string ProfileShortName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public string RolesCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerAccountGroup { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public bool isHasSAPCode { get; set; }
        public string ListProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public Guid? Checker { get; set; }


        public List<ProfileContactProductPromotionViewModel> ProfileContact { get; set; }
        public List<ProfileAddressProductPromotionViewModel> ProfileAddress { get; set; }
        public List<ProfileContactProductPromotionViewModel> ProfileContactActived { get; set; }
        public List<ProfileAddressProductPromotionViewModel> ProfileAddressActived { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
