using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftCampaignEditModel
    {
        public Guid LastEditBy { get; set; }
        public Guid TemplateAndGiftTargetGroupId { get; set; }
        public string TemplateAndGiftCampaignName { get; set; }
    }
}
