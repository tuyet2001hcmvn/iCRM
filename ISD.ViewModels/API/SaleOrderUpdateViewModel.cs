using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SaleOrderUpdateViewModel
    {
        //Master
        public System.Guid SaleOrderId { get; set; }

        [Required]
        public string SaleOrderCode { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string StoreName { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<System.DateTime> PaidDate { get; set; }

        public Nullable<decimal> SubTotal { get; set; }

        public Nullable<decimal> Total { get; set; }

        public string Note { get; set; }

        public Nullable<bool> isUpdatedFromERP { get; set; }

        public Nullable<System.DateTime> UpdatedFromERPTime { get; set; }

        public string SystemNote { get; set; }
    }
}
