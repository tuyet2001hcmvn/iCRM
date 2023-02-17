using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductHierarchyModel", Schema = "ghMasterData")]
    public partial class ProductHierarchyModel
    {
        public ProductHierarchyModel()
        {
            MaterialModels = new HashSet<MaterialModel>();
        }

        [Key]
        [StringLength(50)]
        public string ProductHierarchyCode { get; set; }
        [StringLength(400)]
        public string ProductHierarchyName { get; set; }
        public int? LevelNo { get; set; }
        [StringLength(50)]
        public string ProfitCenterCode { get; set; }
        [StringLength(400)]
        public string ImageUrl { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(MaterialModel.ProductHierarchyCodeNavigation))]
        public virtual ICollection<MaterialModel> MaterialModels { get; set; }
    }
}
