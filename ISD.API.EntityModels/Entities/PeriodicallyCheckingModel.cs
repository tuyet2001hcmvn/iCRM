using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PeriodicallyCheckingModel", Schema = "tSale")]
    public partial class PeriodicallyCheckingModel
    {
        public PeriodicallyCheckingModel()
        {
            ProductPeriodicallyCheckingMappings = new HashSet<ProductPeriodicallyCheckingMapping>();
        }

        [Key]
        public Guid PeriodicallyCheckingId { get; set; }
        [StringLength(50)]
        public string PeriodicallyCheckingCode { get; set; }
        [StringLength(500)]
        public string PeriodicallyCheckingName { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string FileUrl { get; set; }
        public bool? Actived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModifyDate { get; set; }
        [StringLength(100)]
        public string LastModifyUser { get; set; }

        [InverseProperty(nameof(ProductPeriodicallyCheckingMapping.PeriodicallyChecking))]
        public virtual ICollection<ProductPeriodicallyCheckingMapping> ProductPeriodicallyCheckingMappings { get; set; }
    }
}
