using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ExternalCustomerExportModel
    {
        [Display(Name = "Họ và Tên")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại")] 
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
