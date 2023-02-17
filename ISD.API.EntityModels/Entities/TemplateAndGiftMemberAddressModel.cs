using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TemplateAndGiftMemberAddressModel", Schema = "Marketing")]
    public partial class TemplateAndGiftMemberAddressModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? TempalteAndGiftMemberId { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        public Guid? ProductId { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Quantity { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftMemberAddressModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.TemplateAndGiftMemberAddressModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.TemplateAndGiftMemberAddressModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(TempalteAndGiftMemberId))]
        [InverseProperty(nameof(TemplateAndGiftMemberModel.TemplateAndGiftMemberAddressModels))]
        public virtual TemplateAndGiftMemberModel TempalteAndGiftMember { get; set; }
    }
}
