using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialInvoicePriceViewModel
    {
        public System.Guid MaterialPriceId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        //Giá xuất hóa đơn
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InvoicePrice")]
        public decimal? InvoicePrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedFromDate")]
        public DateTime? EffectedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedToDate")]
        public DateTime? EffectedToDate { get; set; }

        public string SAPCode { get; set; }

        public bool? Actived { get; set; }

        public string ProvinceCode { get; set; }
        public string SaleOrg { get; set; }
    }
}
