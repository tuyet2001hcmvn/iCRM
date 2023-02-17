using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class PriceProductViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PriceProductCode")]
        public string PriceProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StyleCode")]
        public string StyleCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StyleName")]
        public string StyleName { get; set; }

        public string MainColorCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ColorName")]
        public string ColorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n0}")]
        public decimal? Price { get; set; }
        
        public DateTime? PostDate { get; set; }
        
        public TimeSpan? PostTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectToDate")]
        public string EffectToDate { get { return string.Format("{0:dd/MM/yyyy} {1:hh\\:mm\\:ss}", PostDate, PostTime); } }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductUserPost")]
        public string UserPost { get; set; }
    }
}
