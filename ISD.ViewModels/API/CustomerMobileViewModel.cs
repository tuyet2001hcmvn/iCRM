using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class CustomerMobileViewModel
    {
        //Customer
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string LastName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string FirstName { get; set; }
        
        public string CMND { get; set; }
        
        public bool? Gender { get; set; }
        
        public string DateOfBirth { get; set; }
        
        public string CustomerAddress { get; set; }
        
        public Guid? District { get; set; }
        
        public Guid? Province { get; set; }
        
        public string Phone { get; set; }
        
        public string EmailAddress { get; set; }

        public string Fax { get; set; }
    }
}
