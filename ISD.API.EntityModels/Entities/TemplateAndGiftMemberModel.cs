using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TemplateAndGiftMemberModel", Schema = "Marketing")]
    public partial class TemplateAndGiftMemberModel
    {
        public TemplateAndGiftMemberModel()
        {
            TemplateAndGiftMemberAddressModels = new HashSet<TemplateAndGiftMemberAddressModel>();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public Guid? ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.TemplateAndGiftMemberModels))]
        public virtual ProfileModel Profile { get; set; }
        [ForeignKey(nameof(TemplateAndGiftTargetGroupId))]
        [InverseProperty(nameof(TemplateAndGiftTargetGroupModel.TemplateAndGiftMemberModels))]
        public virtual TemplateAndGiftTargetGroupModel TemplateAndGiftTargetGroup { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberAddressModel.TempalteAndGiftMember))]
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModels { get; set; }
    }
}
