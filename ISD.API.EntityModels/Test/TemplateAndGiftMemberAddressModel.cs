using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class TemplateAndGiftMemberAddressModel
    {
        public Guid Id { get; set; }
        public Guid? TempalteAndGiftMemberId { get; set; }
        public string Address { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual AccountModel CreateByNavigation { get; set; }
        public virtual AccountModel LastEditByNavigation { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual TemplateAndGiftMemberModel TempalteAndGiftMember { get; set; }
    }
}
