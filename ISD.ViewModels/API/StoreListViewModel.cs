using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class StoreListViewModel
    {
        public Guid StoreId { get; set; }
        public Guid? ProvinceId { get; set; }
        public string StoreName { get; set; }
        public string TelProductTemp { get; set; }
        public string TelProduct
        {
            get
            {
                return string.Format("Tel - Bán hàng: {0}", TelProductTemp);
            }
        }
        public string TelServiceTemp { get; set; }
        public string TelService
        {
            get
            {
                return string.Format("Tel - Dịch vụ: {0}", TelServiceTemp);
            }
        }
        public string StoreAddress { get; set; }
        public string StoreTypeName { get; set; }
        public string ImageUrlTemp { get; set; }
        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainImageStore,
                                                ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage);
            }
        }
        public string LogoUrlTemp{ get; set; }
        public string LogoUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainLogoStore,
                                                LogoUrlTemp != null ? LogoUrlTemp : ConstImageUrl.noImage);
            }
        }
    }
}
