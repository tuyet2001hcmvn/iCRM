using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class FaceCheckInOutViewModel
    {
        //public string FaceId { get; set; }
        //public string PersonName { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.TimeSpan> CheckinTime { get; set; }
        //public string AliasID { get; set; }
        //public string PlaceID { get; set; }
        //public string PersonID { get; set; }
        //public string avatar { get; set; }
        //public string Place { get; set; }
        //public Nullable<int> type { get; set; }
        //public string DeviceID { get; set; }
        //public string DeviceName { get; set;

        public string PersonID { set; get; }
        public string PersonName { get; set; }
        public string Avatar { get; set; }
        public Nullable<System.DateTime> DateCheckIn { get; set; }
        public Nullable<System.TimeSpan> FirstTimeCheckIn { get; set; }
        public Nullable<System.TimeSpan> LastTimeCheckIn { get; set; }

        public string DeviceCheckIn { get; set; }
        public string DeviceCheckOut { get; set; }
    }
}
