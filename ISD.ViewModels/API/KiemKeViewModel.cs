using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class KiemKeViewModel
    {
        public string Warehouse { get; set; }
        public string CurrentUser { get; set; }
        public string CreatedDate { get; set; }

        public List<KiemKeDetailViewModel> detail { get; set; }
    }

    public class KiemKeDetailViewModel
    {
        public string MaterialCode { get; set; }
        public int? Quantity { get; set; }
        public string Batch { get; set; }
        public string MaterialUnit { get; set; }
    }
}
