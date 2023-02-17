using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceOrderTypeModel", Schema = "ghService")]
    public partial class ServiceOrderTypeModel
    {
        [Key]
        [StringLength(50)]
        public string ServiceOrderTypeCode { get; set; }
        [StringLength(200)]
        public string ServiceOrderTypeName { get; set; }
        public int? OrderIndex { get; set; }
    }
}
