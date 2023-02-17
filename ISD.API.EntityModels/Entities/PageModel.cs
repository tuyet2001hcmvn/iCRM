using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PageModel", Schema = "pms")]
    public partial class PageModel
    {
        public PageModel()
        {
            FavoriteReportModels = new HashSet<FavoriteReportModel>();
            PageFunctionModels = new HashSet<PageFunctionModel>();
            PagePermissionModels = new HashSet<PagePermissionModel>();
        }

        [Key]
        public Guid PageId { get; set; }
        [StringLength(100)]
        public string PageName { get; set; }
        [StringLength(300)]
        public string PageUrl { get; set; }
        public Guid? MenuId { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
        [StringLength(100)]
        public string Color { get; set; }
        public bool? Visiable { get; set; }
        [Column("isSystem")]
        public bool? IsSystem { get; set; }
        public bool Actived { get; set; }
        [StringLength(100)]
        public string Parameter { get; set; }

        [ForeignKey(nameof(MenuId))]
        [InverseProperty(nameof(MenuModel.PageModels))]
        public virtual MenuModel Menu { get; set; }
        [InverseProperty(nameof(FavoriteReportModel.Page))]
        public virtual ICollection<FavoriteReportModel> FavoriteReportModels { get; set; }
        [InverseProperty(nameof(PageFunctionModel.Page))]
        public virtual ICollection<PageFunctionModel> PageFunctionModels { get; set; }
        [InverseProperty(nameof(PagePermissionModel.Page))]
        public virtual ICollection<PagePermissionModel> PagePermissionModels { get; set; }
    }
}
