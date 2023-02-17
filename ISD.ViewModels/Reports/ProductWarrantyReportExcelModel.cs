using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProductWarrantyReportExcelModel
    {
        public int? NumberIndex { get; set; }
        public string CompanyName { get; set; }
        public int? ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        public string ProfileShortName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Age { get; set; }
        public string ERPProductCode { get; set; }
        public string ProductName { get; set; }
        public string SerriNo { get; set; }
        public string SaleOrder { get; set; }
        public string OrderDelivery { get; set; }
        public string WarrantyName { get; set; }
        public string ProductWarrantyNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Note { get; set; }
        public string Actived { get; set; }
        public int? ActivatedQuantity { get; set; }
    }
}