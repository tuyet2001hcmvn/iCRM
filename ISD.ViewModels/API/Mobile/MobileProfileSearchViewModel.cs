using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileProfileSearchViewModel
    {
        public string ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string ProfilePhone { get; set; }
        public string TaxNo { get; set; }
        public string CompanyCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string Address { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public Guid? AccountId { get; set; }
    }
}
