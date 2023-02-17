using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileFieldModel", Schema = "Customer")]
    public partial class ProfileFieldModel
    {
        [Key]
        [StringLength(50)]
        public string FieldCode { get; set; }
        [StringLength(50)]
        public string FieldName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int? OrderIndex { get; set; }
    }
}
