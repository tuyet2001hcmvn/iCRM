using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    //Xuất chuyển kho (Reservation)
    public class BarcodeReservationViewModel
    {
        public string Reservation { get; set; }
        //Kho nhận
        public string ImportWarehouse { get; set; }
        //Mã xe
        public string MaterialCode { get; set; }
        //Tên xe
        public string MaterialName { get; set; }
        //Số lượng cần quét (Tổng số xe)
        public int? TotalQuantity { get; set; }
        //ĐVT
        public string MaterialUnit { get; set; }
        //Số lô
        public string Batch { get; set; }
        //Số lượng quét để chuyển SAP
        public int? Quantity { get; set; }
        public string Message { get; set; }
    }
}
