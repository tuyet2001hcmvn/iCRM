using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ServiceOrderUpdateViewModel
    {
        //Master
        public System.Guid ServiceOrderId { get; set; }

        [Required]
        public string ServiceOrderCode { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string StoreName { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string Category { get; set; }

        public string ServicePool { get; set; }

        public string LicensePlate { get; set; }

        public string SerialNumber { get; set; }

        public string EngineNumber { get; set; }

        public Nullable<decimal> CurrentKilometers { get; set; }

        public string PersonnalNumberId { get; set; }

        public Nullable<decimal> Quantity { get; set; }

        public Nullable<decimal> Total { get; set; }

        public string Note { get; set; }

        public Nullable<bool> isUpdatedFromERP { get; set; }

        public Nullable<System.DateTime> UpdatedFromERPTime { get; set; }

        public string SystemNote { get; set; }
    }
}
