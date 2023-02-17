using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessoryDetailModel", Schema = "tSale")]
    public partial class AccessoryDetailModel
    {
        [Key]
        public Guid AccessoryDetailId { get; set; }
        public Guid AccessoryId { get; set; }
        public Guid? HelmetColorId { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(AccessoryId))]
        [InverseProperty(nameof(AccessoryModel.AccessoryDetailModels))]
        public virtual AccessoryModel Accessory { get; set; }
    }
}
