using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SearchCustomerViewModel
    {
        public string CustomerCode { get; set; }
        
        public string FullName { get; set; }
        
        public string CustomerAddress { get; set; }

        public string Phone { get; set; }

        public string Plate { get; set; }

        public string Times { get; set; }
    }
}
