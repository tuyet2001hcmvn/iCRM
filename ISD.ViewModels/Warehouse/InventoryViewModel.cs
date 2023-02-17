using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class InventoryViewModel
    {
        public Guid ProductId { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "StockName")]
        public Guid? StockId { get; set; }
        public string StockName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "ProductName")]
        public string ProductName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "Product_Quantity")]
        [DisplayFormat(DataFormatString = "{0:#}")]
        public decimal Qty { get; set; }
    }
}
