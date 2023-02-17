using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftCampaignCreateModel
    {
        public string TemplateAndGiftCampaignName { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public Guid? CreateBy { get; set; }
    }
}
