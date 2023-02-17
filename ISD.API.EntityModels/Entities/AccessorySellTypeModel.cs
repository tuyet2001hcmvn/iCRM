using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccessorySellTypeModel", Schema = "ghSale")]
    public partial class AccessorySellTypeModel
    {
        [Key]
        public Guid AccessorySellTypeId { get; set; }
        [StringLength(50)]
        public string AccessorySellTypeCode { get; set; }
        [StringLength(200)]
        public string AccessorySellTypeName { get; set; }
        public Guid? ServiceFlagId { get; set; }
        public bool? Actived { get; set; }
        public bool? IsTinhTien { get; set; }
    }
}
