using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class PlateMobileViewModel
    {
        //Customer
        public string LicensePlate { get; set; }
        
        public Guid? CategoryName { get; set; }

        public string SerialNumber { get; set; }

        public string EngineNumber { get; set; }

        public string InvoceDate { get; set; }

        public string Barcode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string UserId { get; set; }
    }
}
