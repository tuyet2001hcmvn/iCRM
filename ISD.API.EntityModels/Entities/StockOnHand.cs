using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ISD.API.EntityModels.Entities
{
    [Table("StockOnHand", Schema = "Warehouse")]
    public class StockOnHand
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ERPProductCode { get; set; }
        public decimal? Qty { get; set; }
    }
}
