using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PagePermissionModel", Schema = "pms")]
    public partial class PagePermissionModel
    {
        [Key]
        public Guid RolesId { get; set; }
        [Key]
        public Guid PageId { get; set; }
        [Key]
        [StringLength(50)]
        public string FunctionId { get; set; }

        [ForeignKey(nameof(FunctionId))]
        [InverseProperty(nameof(FunctionModel.PagePermissionModels))]
        public virtual FunctionModel Function { get; set; }
        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(PageModel.PagePermissionModels))]
        public virtual PageModel Page { get; set; }
        [ForeignKey(nameof(RolesId))]
        [InverseProperty(nameof(RolesModel.PagePermissionModels))]
        public virtual RolesModel Roles { get; set; }
    }
}
