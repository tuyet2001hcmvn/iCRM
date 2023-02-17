using ISD.Constant;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Permission
{
    public class ProductMobileViewModel
    {
        public Guid ProductId { get; set; }
        public Guid? CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public Guid? DefaultColorId { get; set; }
        public Guid? DefaultStyleId { get; set; }
        public string DefaultPrice { get; set; }
        public string DefaultApplyTime { get; set; }
        public int? ProductTypeId { get; set; }
        public string ImageUrlTemp { get; set; }
    }

    public class ColorMobileViewModel
    {
        public Guid ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string ColorRGBCode { get { return ColorCode.ToLower();}}
        public Guid? DefaultStyleId { get; set; }
    }

    public class ImageMobileViewModel
    {
        public string ImageUrlTemp { get; set; }
        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainImageProduct,
                                                ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage);
            }
        }
    }

    public class StyleMobileViewModel
    {
        public Guid? StyleId { get; set; }
        public string StyleName { get; set; }
    }
}
