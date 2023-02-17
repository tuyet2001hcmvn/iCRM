using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PlateFeeModel", Schema = "tSale")]
    public partial class PlateFeeModel
    {
        public PlateFeeModel()
        {
            PlateFeeDetailModels = new HashSet<PlateFeeDetailModel>();
            ProductPlateFeeMappings = new HashSet<ProductPlateFeeMapping>();
        }

        [Key]
        public Guid PlateFeeId { get; set; }
        [StringLength(50)]
        public string PlateFeeCode { get; set; }
        [StringLength(500)]
        public string PlateFeeName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public bool? Actived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModifyDate { get; set; }
        [StringLength(100)]
        public string LastModifyUser { get; set; }

        [InverseProperty(nameof(PlateFeeDetailModel.PlateFee))]
        public virtual ICollection<PlateFeeDetailModel> PlateFeeDetailModels { get; set; }
        [InverseProperty(nameof(ProductPlateFeeMapping.PlateFee))]
        public virtual ICollection<ProductPlateFeeMapping> ProductPlateFeeMappings { get; set; }
    }
}
