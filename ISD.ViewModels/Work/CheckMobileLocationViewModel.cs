using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CheckMobileLocationViewModel
    {
        public Guid AccountId { get; set; }
        public Guid TaskId { get; set; }
        public Guid? CheckInOutId { get; set; }
        //location of task
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        //location of mobile
        public double CurrentLatitude { get; set; }
        public double CurrentLongtitude { get; set; }
        public double Distance { get; set; }
        public bool? isCheckOut { get; set; }
    }
}
