using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Permission
{
    public class BannerMobileViewModel
    {
        public string ImageUrlTemp { get; set; }
        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainBanner, ImageUrlTemp );
            }
        }
        public DateTime? CreatedTime { get; set; }
    }
}
