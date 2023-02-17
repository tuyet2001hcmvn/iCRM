using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ServiceOrderUpdateDetailViewModel
    {
        //Detail
        public System.Guid ServiceOrderDetailId { get; set; }

        public Guid ServiceOrderId { get; set; }

        public string Description { get; set; }

        public Nullable<decimal> Price { get; set; }

        public Nullable<decimal> Quantity { get; set; }

        public Nullable<bool> DiscountType { get; set; }

        public Nullable<decimal> Discount { get; set; }

        public Nullable<decimal> UnitPrice { get; set; }
    }
}
