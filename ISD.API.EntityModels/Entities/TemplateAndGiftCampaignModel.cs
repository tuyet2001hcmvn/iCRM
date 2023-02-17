using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TemplateAndGiftCampaignModel", Schema = "Marketing")]
    public partial class TemplateAndGiftCampaignModel
    {
        [Key]
        public Guid Id { get; set; }
        public int TemplateAndGiftCampaignCode { get; set; }
        [StringLength(50)]
        public string TemplateAndGiftCampaignName { get; set; }
        public Guid? TemplateAndGiftTargetGroupId { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftCampaignModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftCampaignModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [ForeignKey(nameof(TemplateAndGiftTargetGroupId))]
        [InverseProperty(nameof(TemplateAndGiftTargetGroupModel.TemplateAndGiftCampaignModels))]
        public virtual TemplateAndGiftTargetGroupModel TemplateAndGiftTargetGroup { get; set; }
    }
}
