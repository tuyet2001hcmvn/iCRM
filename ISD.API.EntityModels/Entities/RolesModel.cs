using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RolesModel", Schema = "pms")]
    public partial class RolesModel
    {
        public RolesModel()
        {
            AccountInRoleModels = new HashSet<AccountInRoleModel>();
            MobileScreenPermissionModels = new HashSet<MobileScreenPermissionModel>();
            PagePermissionModels = new HashSet<PagePermissionModel>();
            RoleInChargeModels = new HashSet<RoleInChargeModel>();
        }

        [Key]
        public Guid RolesId { get; set; }
        [StringLength(50)]
        public string RolesCode { get; set; }
        [Required]
        [StringLength(50)]
        public string RolesName { get; set; }
        [Column("isCheckLoginByTime")]
        public bool? IsCheckLoginByTime { get; set; }
        [Column("isEmployeeGroup")]
        public bool? IsEmployeeGroup { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? WorkingTimeFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? WorkingTimeTo { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }
        [Column("isSendSMSPermission")]
        public bool? IsSendSmspermission { get; set; }

        [InverseProperty(nameof(AccountInRoleModel.Roles))]
        public virtual ICollection<AccountInRoleModel> AccountInRoleModels { get; set; }
        [InverseProperty(nameof(MobileScreenPermissionModel.Roles))]
        public virtual ICollection<MobileScreenPermissionModel> MobileScreenPermissionModels { get; set; }
        [InverseProperty(nameof(PagePermissionModel.Roles))]
        public virtual ICollection<PagePermissionModel> PagePermissionModels { get; set; }
        [InverseProperty(nameof(RoleInChargeModel.Roles))]
        public virtual ICollection<RoleInChargeModel> RoleInChargeModels { get; set; }
    }
}
