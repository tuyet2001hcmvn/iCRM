using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MobileScreenModel", Schema = "ghMasterData")]
    public partial class MobileScreenModel
    {
        public MobileScreenModel()
        {
            MobileScreenFunctionModels = new HashSet<MobileScreenFunctionModel>();
            MobileScreenPermissionModels = new HashSet<MobileScreenPermissionModel>();
        }

        [Key]
        public Guid MobileScreenId { get; set; }
        [StringLength(100)]
        public string ScreenName { get; set; }
        [StringLength(300)]
        public string ScreenCode { get; set; }
        public Guid? MenuId { get; set; }
        [StringLength(50)]
        public string IconType { get; set; }
        [StringLength(50)]
        public string IconName { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
        public bool? Visible { get; set; }
        [Column("isSystem")]
        public bool? IsSystem { get; set; }
        public bool? Actived { get; set; }
        [Column("isCreated")]
        public bool? IsCreated { get; set; }
        [Column("isReporter")]
        public bool? IsReporter { get; set; }
        [Column("isAssignee")]
        public bool? IsAssignee { get; set; }

        [ForeignKey(nameof(MenuId))]
        [InverseProperty(nameof(MenuModel.MobileScreenModels))]
        public virtual MenuModel Menu { get; set; }
        [InverseProperty(nameof(MobileScreenFunctionModel.MobileScreen))]
        public virtual ICollection<MobileScreenFunctionModel> MobileScreenFunctionModels { get; set; }
        [InverseProperty(nameof(MobileScreenPermissionModel.MobileScreen))]
        public virtual ICollection<MobileScreenPermissionModel> MobileScreenPermissionModels { get; set; }
    }
}
