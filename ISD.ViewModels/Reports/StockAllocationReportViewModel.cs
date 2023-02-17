using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockAllocationReportViewModel
    {
        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }
        [Display(Name = "Tên chi nhánh")]
        public string StoreName { get; set; }
        [Display(Name = "Nhóm vật tư")]
        public string CategoryName { get; set; }
        [Display(Name = "Catalog")]
        public decimal CatalogueRealityQuantity { get; set; }
        [Display(Name = "Mẫu")]
        public decimal SampleRealityQuantity { get; set; }
        [Display(Name = "Kệ và Vật tư")]
        public decimal KeRealityQuantity { get; set; }
        [Display(Name = "Brochure")]
        public decimal BrochureRealityQuantity { get; set; }
        [Display(Name = "Bao bì")]
        public decimal PakingRealityQuantity { get; set; }
        [Display(Name = "Kế hoạch")]
        public decimal ExpectedQuantity { get; set; }
        [Display(Name = "Thực tế phân bổ")]
        public decimal TotalValue { get; set; }
        [Display(Name = "Tỉ lệ đã phân bổ")]
        public decimal Ratio { get; set; }
        
    }
}
