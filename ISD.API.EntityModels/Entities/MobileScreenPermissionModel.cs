using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MobileScreenPermissionModel", Schema = "pms")]
    public partial class MobileScreenPermissionModel
    {
        [Key]
        public Guid RolesId { get; set; }
        [Key]
        public Guid MobileScreenId { get; set; }
        [Key]
        [StringLength(50)]
        public string FunctionId { get; set; }

        [ForeignKey(nameof(FunctionId))]
        [InverseProperty(nameof(FunctionModel.MobileScreenPermissionModels))]
        public virtual FunctionModel Function { get; set; }
        [ForeignKey(nameof(MobileScreenId))]
        [InverseProperty(nameof(MobileScreenModel.MobileScreenPermissionModels))]
        public virtual MobileScreenModel MobileScreen { get; set; }
        [ForeignKey(nameof(RolesId))]
        [InverseProperty(nameof(RolesModel.MobileScreenPermissionModels))]
        public virtual RolesModel Roles { get; set; }
    }
}
