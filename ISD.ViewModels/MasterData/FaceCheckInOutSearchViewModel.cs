using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class FaceCheckInOutSearchViewModel
    {
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }

        public string FaceType { set; get; }
    }
}
