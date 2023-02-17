using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TemplateAndGiftTargetGroupModel", Schema = "Marketing")]
    public partial class TemplateAndGiftTargetGroupModel
    {
        public TemplateAndGiftTargetGroupModel()
        {
            TemplateAndGiftCampaignModels = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftMemberModels = new HashSet<TemplateAndGiftMemberModel>();
        }

        [Key]
        public Guid Id { get; set; }
        public int TargetGroupCode { get; set; }
        [StringLength(50)]
        public string TargetGroupName { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public bool? Actived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftTargetGroupModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftTargetGroupModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [InverseProperty(nameof(TemplateAndGiftCampaignModel.TemplateAndGiftTargetGroup))]
        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModels { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberModel.TemplateAndGiftTargetGroup))]
        public virtual ICollection<TemplateAndGiftMemberModel> TemplateAndGiftMemberModels { get; set; }
    }
}
