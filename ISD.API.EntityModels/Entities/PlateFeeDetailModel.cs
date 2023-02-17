using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PlateFeeDetailModel", Schema = "tSale")]
    public partial class PlateFeeDetailModel
    {
        [Key]
        public Guid PlateFeeDetailId { get; set; }
        public Guid? PlateFeeId { get; set; }
        [StringLength(50)]
        public string Province { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Price { get; set; }

        [ForeignKey(nameof(PlateFeeId))]
        [InverseProperty(nameof(PlateFeeModel.PlateFeeDetailModels))]
        public virtual PlateFeeModel PlateFee { get; set; }
    }
}
