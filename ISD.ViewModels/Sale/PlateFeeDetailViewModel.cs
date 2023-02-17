using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class PlateFeeDetailViewModel
    {
        public int STT { get; set; }
        public System.Guid PlateFeeDetailId { get; set; }

        public Nullable<System.Guid> PlateFeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public string Province { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        public string Description { get; set; }
    }
    public class PlateFeeDetailAPIViewModel
    {
        public string Province { get; set; }
        public Nullable<decimal> PriceTmp { protected get; set; }
        public string Price { get {
                return string.Format("{0:n0}đ", PriceTmp);
            } }
    }
}
