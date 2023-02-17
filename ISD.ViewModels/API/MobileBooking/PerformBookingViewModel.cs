using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API.MobileBooking
{

    public class PerformBookingViewModel
    {

        //1. - Mã chi nhánh
        public Guid StoreId { get; set; }
        //2. - Mã KH
        public Guid CustomerId { get; set; }
        //3. - Biển số xe
        public string Plate { get; set; }
        //4. - Loại xe
        public string BikeModel { get; set; }
        //5. - Tên KH
        public string FullName { get; set; }
        //6. - Số ĐT
        public string Phone { get; set; }
        //7. - Ngày đặt
        public string BookingDate { get; set; }
        //8. - Giờ đặt
        public string BookingTime { get; set; }
        public long BookingCode { get; set; }
    }
}
