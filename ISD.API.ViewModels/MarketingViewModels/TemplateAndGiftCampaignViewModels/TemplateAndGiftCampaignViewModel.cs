using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftCampaignViewModel
    {
        public Guid Id { get; set; }
        public int TemplateAndGiftCampaignCode { get; set; }
        public string TemplateAndGiftCampaignName { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
