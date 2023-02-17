using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class OrderDetailViewModel
    {
        public string OrderDelivery { get; set; }
        public string CompanyCode { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? PostDate { get; set; }

        //Hang mục OD
        public string OrderDeliveryCategory { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; }
        public string Batch { get; set; }
        public string SerialNo { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCategoryName { get; set; }
        public string WarrantyCode { get; set; }
        public string WarrantyUnit { get; set; }
        public string SaleOrderCode { get; set; }
        public string SaleOrderCodeCategory { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        public string ProfileAddress { get; set; }
        public string Phone { get; set; }
    }
}
