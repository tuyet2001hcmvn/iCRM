using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class FaceCheckInOutCreateViewModel
    {
        public string Token { set; get; }
        public string Name { set; get; }
        public string File { set; get; }
        public string AliasID { set; get; }
        public string PlaceID { set; get; }
        public string Title { set; get; }
        public int Type { set; get; }
    }
}
