using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Stock_Store_Mapping", Schema = "Warehouse")]
    public partial class StockStoreMapping
    {
        [Key]
        public Guid StockId { get; set; }
        [Key]
        public Guid StoreId { get; set; }
        [Column("isMain")]
        public bool? IsMain { get; set; }

        [ForeignKey(nameof(StockId))]
        [InverseProperty(nameof(StockModel.StockStoreMappings))]
        public virtual StockModel Stock { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.StockStoreMappings))]
        public virtual StoreModel Store { get; set; }
    }
}
