using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockOnHandReportSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Guid? SearchCompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid? SearchStoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        public Guid? SearchStockId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCode")]
        public string ProductCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProductCodeSearch")]
        public string ProductCodeSearch { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
        public List<Guid> SearchProductId { get; set; }
        public bool IsView { get; set; }
    }
}
