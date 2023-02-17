using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class AccessoryListViewModel
    {
        public Guid AccessoryCategoryId { get; set; }
        public Guid AccessoryId { get; set; }
        public string AccessoryCategoryCode { get; set; }
        public string AccessoryName { get; set; }
        public decimal? Price { get; set; }
        public string PriceWithFormat
        {
            get { return string.Format("{0:n0}đ", Price); }
        }
        public bool? isHelmet { get; set; }
        public bool? isHelmetAdult { get; set; }
        public string Size { get; set; }
        public string ImageUrlTemp { protected get; set; }
        public string ImageUrl { get { return ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage; } }

        public List<AccessoryDetailListViewModel> AccessoryDetail { get; set; }
    }
    public class AccessoryDetailListViewModel
    {
        public Guid? HelmetColorId { get; set; }
        public string ImageUrl { get; set; }
        public string ColorRGBCodeTemp { get; set; }
        public string ColorRGBCode { get { return ColorRGBCodeTemp.ToLower(); } }
    }

    public class AccessorCategoryListViewModel
    {
        public Guid AccessoryCategoryId { get; set; }
        public string AccessoryCategoryName { get; set; }
        public string ImageUrlTemp { get; set; }
        public string ImageUrl { get { return string.Format("{0}/{1}", ConstDomain.DomainImageAccessoryCategory, ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage); } }
    }
}
