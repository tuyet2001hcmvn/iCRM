using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerReviewsReportModel
    {
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string SaleEmployeeName { get; set; }
        public int VoteQty1 { get; set; }
        public int VoteQty2 { get; set; }
        public int VoteQty3 { get; set; }
        public int VoteQty4 { get; set; }
        public int VoteQty5 { get; set; }
        //Ý kiến khác
        public int VoteQty6 { get; set; }
        public decimal Avg { get; set; }
    }
}
