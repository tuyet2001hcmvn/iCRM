using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class BookingListViewModel
    {
        public long? BookingCode { get; set; }
        public DateTime? BookingDate { get; set; }
        public TimeSpan? BookingTimeTemp { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Plate { get; set; }
        public string BookingTime
        {
            get
            {
                return string.Format("{0:00}:{1:00}", BookingTimeTemp.Value.Hours, BookingTimeTemp.Value.Minutes);
            }
        }
    }
}
