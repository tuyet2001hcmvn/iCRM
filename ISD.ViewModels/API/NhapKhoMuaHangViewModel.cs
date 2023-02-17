using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class NhapKhoMuaHangViewModel
    {
        public string PONumber { get; set; }
        public string Warehouse { get; set; }
        public string CurrentUser { get; set; }
        public string CreatedDate { get; set; }

        public List<BarcodePONumberViewModel> detail { get; set; }
    }
}
