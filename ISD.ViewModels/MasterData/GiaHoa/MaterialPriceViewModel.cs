using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialPriceViewModel
    {
        public System.Guid MaterialPriceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        //Giá xuất hóa đơn
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InvoicePrice")]
        public decimal? InvoicePrice { get; set; }

        //Giá tính lệ phí trước bạ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RegistrationFeePrice")]
        public decimal? RegistrationFeePrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedFromDate")]
        public DateTime? EffectedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedToDate")]
        public DateTime? EffectedToDate { get; set; }
    }
}
