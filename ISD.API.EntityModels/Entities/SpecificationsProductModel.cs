using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SpecificationsProductModel", Schema = "tSale")]
    public partial class SpecificationsProductModel
    {
        [Key]
        public Guid SpecificationsProductId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SpecificationsId { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductModel.SpecificationsProductModels))]
        public virtual ProductModel Product { get; set; }
        [ForeignKey(nameof(SpecificationsId))]
        [InverseProperty(nameof(SpecificationsModel.SpecificationsProductModels))]
        public virtual SpecificationsModel Specifications { get; set; }
    }
}
