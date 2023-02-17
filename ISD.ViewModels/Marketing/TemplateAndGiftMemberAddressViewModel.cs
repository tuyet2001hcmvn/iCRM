using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TemplateAndGiftMemberAddressViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Profile_ProfileCode")]
        public string ProfileCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string ProfileForeignCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string ProfileName { get; set; }
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }
        [Display(Name = "Địa chỉ")]
        public string ProductCode { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Số lượng")]
        public string Quantity { get; set; }
    }
}
