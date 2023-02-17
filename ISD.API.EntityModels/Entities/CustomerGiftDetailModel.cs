using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerGiftDetailModel", Schema = "tMasterData")]
    public partial class CustomerGiftDetailModel
    {
        [Key]
        public Guid GiftId { get; set; }
        [Key]
        public Guid CustomerId { get; set; }
        [Column("isRead")]
        public bool? IsRead { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(CustomerModel.CustomerGiftDetailModels))]
        public virtual CustomerModel Customer { get; set; }
        [ForeignKey(nameof(GiftId))]
        [InverseProperty(nameof(CustomerGiftModel.CustomerGiftDetailModels))]
        public virtual CustomerGiftModel Gift { get; set; }
    }
}
