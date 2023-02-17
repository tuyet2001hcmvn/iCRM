using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProductTypeModel", Schema = "tSale")]
    public partial class ProductTypeModel
    {
        [Key]
        public int ProductTypeId { get; set; }
        [StringLength(50)]
        public string ProductTypeName { get; set; }
    }
}
