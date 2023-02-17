using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class TemplateAndGiftMemberModel
    {
        public TemplateAndGiftMemberModel()
        {
            TemplateAndGiftMemberAddressModels = new HashSet<TemplateAndGiftMemberAddressModel>();
        }

        public Guid Id { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public Guid? ProfileId { get; set; }

        public virtual ProfileModel Profile { get; set; }
        public virtual TemplateAndGiftTargetGroupModel TemplateAndGiftTargetGroup { get; set; }
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModels { get; set; }
    }
}
