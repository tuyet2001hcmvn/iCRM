using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CostOfUsingCatalogueComparedToApprovalViewModel
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public string SaleOfficeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ProductGroups")]
        public string ProductGroups { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ProductName")]
        public string ProductName { get; set; }
        //Đề xuất - Số lượng
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferQuantity { get; set; }
        //Đề xuất - Giá
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SampleValue")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferSampleValueMin { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SampleValue")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferSampleValueMax { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProcessingValue")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferProcessingValueMin { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SampleValue")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferProcessingValueMax { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UnitPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferUnitPriceMin { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UnitPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferUnitPriceMax { get; set; }
        //Đề xuất - Thành tiền
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? OfferPrice { get; set; }

        //Được duyệt - Số lượng
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? RequestQuantity { get; set; }
        //ĐƯợc duyệt - thành tiền
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? RequestPrice { get; set; }

        //Được duyệt - Số lượng
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? TransferredQuantity { get; set; }
        //ĐƯợc duyệt - thành tiền
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? TransferredPrice { get; set; }


        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? DifferenceQuantity { get {
                decimal? res = 0;
                res = RequestQuantity - TransferredQuantity;
                return res;
            }
        }
        //ĐƯợc duyệt - thành tiền
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? DifferencePrice {
            get
            {
                decimal? res = 0;
                res = RequestPrice - TransferredPrice;
                return res;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeName { get; set; }
    }
    public class CostOfUsingCatalogueComparedToApprovalSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> SaleOfficeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompanyId")]
        public Guid? CompanyId { get; set; } 
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StoreId")]
        public Guid? StoreId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product")]
        public List<Guid> ProductId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public List<string> RolesCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public List<string> SalesEmployeeCode { get; set; }
        [Display(Name = "Loại giá")]
        public string PriceType { get; set; }

        //13. Thời gian
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }

        public string ReportType { get; set; }
    }
}
