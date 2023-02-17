using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerQuantityReportViewModel
    {
        //Nhân viên
        [Display(Name = "Nhân viên")]
        public string Employee { get; set; }

        //Chi nhánh/NPP/SRNQ
        [Display(Name = "Chi nhánh/NPP/SRNQ")]
        public string SaleOrg { get; set; }

        //Nguồn khách hàng(ShowRoom/QRNQ)
        [Display(Name = "Nguồn khách hàng(ShowRoom/QRNQ)")]
        public string CustomerSource { get; set; }

        //Số lượng khách
        [Display(Name = "Số lượng khách")]
        public int CustomerQuantity { get; set; }
    }
}
