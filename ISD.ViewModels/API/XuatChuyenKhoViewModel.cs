using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class XuatChuyenKhoViewModel
    {
        public string Reservation { get; set; }
        public string ImportWarehouse { get; set; }
        public string ExportWarehouse { get; set; }
        public string CurrentUser { get; set; }
        public string CreatedDate { get; set; }

        public List<BarcodeReservationViewModel> detail { get; set; }
    }
}
