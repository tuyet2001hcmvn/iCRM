using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Marketing
{
    public class CampaignSearchViewModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignCode")]
        public int CampaignCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampaignName")]
        public string CampaignName { get; set; }    

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentName")]
        public string ContentName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TargetGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScheduledToStart")]
        public DateTime ScheduledToStart { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }
        public string Type { get; set; }
    }
}
