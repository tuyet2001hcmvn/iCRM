using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ModuleModel", Schema = "pms")]
    public partial class ModuleModel
    {
        public ModuleModel()
        {
            MenuModels = new HashSet<MenuModel>();
        }

        [Key]
        public Guid ModuleId { get; set; }
        [StringLength(100)]
        public string ModuleName { get; set; }
        [StringLength(100)]
        public string Url { get; set; }
        [Column("isSystemModule")]
        public bool? IsSystemModule { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(4000)]
        public string Details { get; set; }

        [InverseProperty(nameof(MenuModel.Module))]
        public virtual ICollection<MenuModel> MenuModels { get; set; }
    }
}
