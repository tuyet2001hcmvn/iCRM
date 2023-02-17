using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API.MobileBooking
{
    public class BookingTimeViewModel
    {
        public TimeSpan? TimeSpan { protected get; set; }
        public string Time { get {
                return string.Format("{0:D2}:{1:D2}", TimeSpan.Value.Hours, TimeSpan.Value.Minutes);
            } }
        public int Number { get; set; }

    }
}
