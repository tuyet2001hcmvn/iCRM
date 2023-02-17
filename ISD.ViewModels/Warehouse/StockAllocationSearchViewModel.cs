using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockAllocationSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Guid? SearchCompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid? SearchStoreId { get; set; }
        ////Loại Catalogue
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        //public Guid?  SearchCategoryId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "DocumentDate")]
        public DateTime? SearchToDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "DocumentDate")]
        public DateTime? SearchFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Brochure_EpectedQuantity")]
        [Required]
        public decimal BrochureEpectedQuantity { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ExpectedQuantity")]
        [Required]
        public decimal CatalogueExpectedQuantity { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KE_ExpectedQuantity")]
        [Required]
        public decimal KEExpectedQuantity { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Packaging_ExpectedQuantity")]
        [Required]
        public decimal PackagingExpectedQuantity { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sample_ExpectedQuantity")]
        [Required]
        public decimal SampleExpectedQuantity { get; set; }
        public bool IsView { get; set; }
        public bool? IsExport { get; set; }
    }
}
