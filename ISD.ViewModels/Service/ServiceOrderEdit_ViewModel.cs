using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ServiceOrderEdit_ViewModel
    {
        public Guid ServiceOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string ServiceOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string Category { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_ServicePool")]
        public string ServicePool { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_CurrentKm")]
        public decimal? CurrentKilometers { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_PersonnalNumberId")]
        public string PersonnalNumberId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_Quantity")]
        public Nullable<decimal> Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        public decimal? Total { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SystemNote")]
        public string SystemNote { get; set; }

        public List<ServiceOrderDetailViewModel> detailList { get; set; }

        //Detail
        public class ServiceOrderDetailViewModel
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Price")]
            public Nullable<decimal> Price { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
            public Nullable<decimal> Quantity { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DiscountType")]
            public Nullable<bool> DiscountType { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Discount")]
            public Nullable<decimal> Discount { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_UnitPrice")]
            public Nullable<decimal> UnitPrice { get; set; }
        }
    }
}
