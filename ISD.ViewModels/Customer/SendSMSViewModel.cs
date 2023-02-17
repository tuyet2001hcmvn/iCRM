using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class SendSMSViewModel
    {
        public string PhoneNumber { get; set; }
        public string BrandName { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
