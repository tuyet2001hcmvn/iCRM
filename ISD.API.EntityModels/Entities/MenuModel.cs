using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MenuModel", Schema = "pms")]
    public partial class MenuModel
    {
        public MenuModel()
        {
            MobileScreenModels = new HashSet<MobileScreenModel>();
            PageModels = new HashSet<PageModel>();
        }

        [Key]
        public Guid MenuId { get; set; }
        public Guid? ModuleId { get; set; }
        [StringLength(100)]
        public string MenuName { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }

        [ForeignKey(nameof(ModuleId))]
        [InverseProperty(nameof(ModuleModel.MenuModels))]
        public virtual ModuleModel Module { get; set; }
        [InverseProperty(nameof(MobileScreenModel.Menu))]
        public virtual ICollection<MobileScreenModel> MobileScreenModels { get; set; }
        [InverseProperty(nameof(PageModel.Menu))]
        public virtual ICollection<PageModel> PageModels { get; set; }
    }
}
