using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WarehouseModel", Schema = "tSale")]
    public partial class WarehouseModel
    {
        public WarehouseModel()
        {
            WarehouseProductModels = new HashSet<WarehouseProductModel>();
        }

        [Key]
        public Guid WarehouseId { get; set; }
        [Required]
        [StringLength(50)]
        public string WarehouseCode { get; set; }
        [Required]
        [StringLength(200)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string WarehouseShortName { get; set; }
        public Guid StoreId { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }

        [InverseProperty(nameof(WarehouseProductModel.Warehouse))]
        public virtual ICollection<WarehouseProductModel> WarehouseProductModels { get; set; }
    }
}
