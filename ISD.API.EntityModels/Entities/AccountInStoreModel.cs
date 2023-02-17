using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccountInStoreModel", Schema = "pms")]
    public partial class AccountInStoreModel
    {
        [Key]
        public Guid AccountId { get; set; }
        [Key]
        public Guid StoreId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(AccountModel.AccountInStoreModels))]
        public virtual AccountModel Account { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.AccountInStoreModels))]
        public virtual StoreModel Store { get; set; }
    }
}
