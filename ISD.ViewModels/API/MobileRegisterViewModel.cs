using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class MobileRegisterViewModel
    {
        public string FullName { get; set; }
        public string OTPCode { get; set; }
        public string ErrorMessage { get; set; }
        public string CustomerCode { get; set; }
        public bool Result { get; set; }
    }
}
