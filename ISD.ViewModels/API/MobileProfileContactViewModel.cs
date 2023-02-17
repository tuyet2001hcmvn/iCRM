using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileProfileContactViewModel
    {
        public Guid? ProfileContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPosition { get; set; }
        public string ContactDepartment { get; set; }
        public bool? IsMain { get; set; }
    }
}
