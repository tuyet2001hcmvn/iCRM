using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Permission
{
    public class CustomerPromotionViewModel
    {
        public DateTime? EffectToDateTemp { get; set; }
        public string EffectToDate
        {
            get
            {
                return string.Format("Hiệu lực đến: {0}", string.Format("{0: dd/MM/yyyy}", EffectToDateTemp));
            }
        }
        public string PromotionName { get; set; }
        public string PromotionCode { get; set; }
        public string Description { get; set; }
        public string ImageUrlTemp { get; set; }
        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainImageCustomerPromotion,
                                                ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage);
            }
        }
    }
}
