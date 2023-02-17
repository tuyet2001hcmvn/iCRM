using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class TemplateAndGiftCampaignModel
    {
        public Guid Id { get; set; }
        public int TemplateAndGiftCampaignCode { get; set; }
        public string TemplateAndGiftCampaignName { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual AccountModel CreateByNavigation { get; set; }
        public virtual AccountModel LastEditByNavigation { get; set; }
        public virtual TemplateAndGiftTargetGroupModel TemplateAndGiftTargetGroup { get; set; }
    }
}
