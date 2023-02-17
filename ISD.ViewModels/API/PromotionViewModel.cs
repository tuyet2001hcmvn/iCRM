using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class PromotionViewModel
    {
        public string PromotionName { get; set; }
        public DateTime? EffectToDate { protected get; set; }
        public string EffectToDateWithFormat { get { return string.Format("{0:dd/MM/yyy}", EffectToDate); } }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
