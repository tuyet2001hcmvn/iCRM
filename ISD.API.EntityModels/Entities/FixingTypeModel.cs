using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("FixingTypeModel", Schema = "ghService")]
    public partial class FixingTypeModel
    {
        [Key]
        public Guid FixingTypeId { get; set; }
        [StringLength(50)]
        public string FixingTypeCode { get; set; }
        [StringLength(200)]
        public string FixingTypeName { get; set; }
        public Guid? ServiceFlagId { get; set; }
        public bool? Actived { get; set; }
        public bool? IsTinhTien { get; set; }
        public bool? IsBaoHanh { get; set; }
        public bool? IsKhieuNai { get; set; }
        public bool? IsAccessory { get; set; }
    }
}
