using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ProspectCustomerViewModel
    {
        //public string CustomerCode { get; set; }
        
        public string LastName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string FirstName { get; set; }

        public string CustomerName { get; set; }

        public string CMND { get; set; }
        
        public bool? Gender { get; set; }
        
        public string DateOfBirth { get; set; }
        
        public string CustomerAddress { get; set; }
        
        public Guid? District { get; set; }
        
        public Guid? Province { get; set; }
        
        public string Phone { get; set; }
        
        public string EmailAddress { get; set; }

        //Opportunities
        public bool isOpportunities { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Owner { get; set; }

        public Guid? Prognosis { get; set; }

        public string Probability { get; set; }

        public Guid? SalesUnit { get; set; }

        public Guid? SalesProcess { get; set; }

        public string EstimatedRevenue { get; set; }

        public Guid? SourceId { get; set; }

        //Mã NV
        public string HcmPersonnelNumberId { get; set; }
    }
}
