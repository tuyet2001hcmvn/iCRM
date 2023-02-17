using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccountInRoleModel", Schema = "pms")]
    public partial class AccountInRoleModel
    {
        [Key]
        public Guid AccountId { get; set; }
        [Key]
        public Guid RolesId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(AccountModel.AccountInRoleModels))]
        public virtual AccountModel Account { get; set; }
        [ForeignKey(nameof(RolesId))]
        [InverseProperty(nameof(RolesModel.AccountInRoleModels))]
        public virtual RolesModel Roles { get; set; }
    }
}
