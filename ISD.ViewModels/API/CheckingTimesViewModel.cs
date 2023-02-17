using ISD.ViewModels.API;
using ISD.ViewModels.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class NotificationViewModel
    {
        public List<CheckingTimesNotificationViewModel> CheckingTimes { get; set; }
        public List<CustomerGiftAPIViewModel> Gifts { get; set; }
    }
    public class CheckingTimesViewModel
    {
        public string Plate { get; set; }
        
        public string Configuration { get; set; }

        public string Description { get; set; }
    }
}
