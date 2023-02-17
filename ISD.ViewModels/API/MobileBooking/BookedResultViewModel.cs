using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API.MobileBooking
{
    public class BookedResultViewModel
    {
        ///1. - Mã phiếu đặt
        public long BookingCode { get; set; }
        ///2. - Biển số xe
        public string Plate { get; set; }
        ///3. - Ngày đặt
        public string BookingDate { get {

                if (BookingDateTmp == null)
                {
                    return "";
                }
                else
                {
                    return string.Format("{0:dd/MM/yyyy}", BookingDateTmp);
                }
            } }
        public DateTime? BookingDateTmp { protected get; set; }
        
        ///4. - Giờ đặt
        public string BookingTime
        {
            get
            {
                if (BookingTimeTmp == null)
                {
                    return "";
                }
                else
                {
                    return string.Format("{0:D2}:{1:D2}", BookingTimeTmp.Value.Hours, BookingTimeTmp.Value.Minutes);
                }
            }
        }
        public TimeSpan? BookingTimeTmp { protected get; set; }

        ///5. - Trạng thái
        /// 5.1. Tên trạng thái
        public string StatusName { get; set; }
        /// 5.2. Loại trạng thái
        public bool? Status { get; set; }
        ///6. - Chi nhánh
        public string StoreName { get; set; }
    }
}
