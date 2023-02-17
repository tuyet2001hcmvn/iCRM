using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("FunctionModel", Schema = "pms")]
    public partial class FunctionModel
    {
        public FunctionModel()
        {
            MobileScreenFunctionModels = new HashSet<MobileScreenFunctionModel>();
            MobileScreenPermissionModels = new HashSet<MobileScreenPermissionModel>();
            PageFunctionModels = new HashSet<PageFunctionModel>();
            PagePermissionModels = new HashSet<PagePermissionModel>();
        }

        [Key]
        [StringLength(50)]
        public string FunctionId { get; set; }
        [StringLength(50)]
        public string FunctionName { get; set; }
        public int? OrderIndex { get; set; }

        [InverseProperty(nameof(MobileScreenFunctionModel.Function))]
        public virtual ICollection<MobileScreenFunctionModel> MobileScreenFunctionModels { get; set; }
        [InverseProperty(nameof(MobileScreenPermissionModel.Function))]
        public virtual ICollection<MobileScreenPermissionModel> MobileScreenPermissionModels { get; set; }
        [InverseProperty(nameof(PageFunctionModel.Function))]
        public virtual ICollection<PageFunctionModel> PageFunctionModels { get; set; }
        [InverseProperty(nameof(PagePermissionModel.Function))]
        public virtual ICollection<PagePermissionModel> PagePermissionModels { get; set; }
    }
}
