using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RoleInChargeModel", Schema = "Customer")]
    public partial class RoleInChargeModel
    {
        [Key]
        public Guid RoleInChargeId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? RolesId { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.RoleInChargeModels))]
        public virtual ProfileModel Profile { get; set; }
        [ForeignKey(nameof(RolesId))]
        [InverseProperty(nameof(RolesModel.RoleInChargeModels))]
        public virtual RolesModel Roles { get; set; }
    }
}
