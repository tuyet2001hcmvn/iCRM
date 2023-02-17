using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class TemplateAndGiftTargetGroupModel
    {
        public TemplateAndGiftTargetGroupModel()
        {
            TemplateAndGiftCampaignModels = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftMemberModels = new HashSet<TemplateAndGiftMemberModel>();
        }

        public Guid Id { get; set; }
        public int TargetGroupCode { get; set; }
        public string TargetGroupName { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public bool? Actived { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual AccountModel CreateByNavigation { get; set; }
        public virtual AccountModel LastEditByNavigation { get; set; }
        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModels { get; set; }
        public virtual ICollection<TemplateAndGiftMemberModel> TemplateAndGiftMemberModels { get; set; }
    }
}
