using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerTasteSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
        public Guid? CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StoreId")]
        public Guid? StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string ProductCode { get; set; }

        public string SaleOrgCode { get; set; }
    }
}
