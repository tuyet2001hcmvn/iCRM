using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ExternalCustomerSearchModel
    {
        public string CompanyCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}
