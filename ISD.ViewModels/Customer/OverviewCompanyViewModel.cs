using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class OverviewCompanyViewModel
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerGroup { get; set; }
        public string PersonInCharge { get; set; }
    }
}
