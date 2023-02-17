using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileCategoryModel", Schema = "Customer")]
    public partial class ProfileCategoryModel
    {
        [Key]
        [StringLength(50)]
        public string ProfileCategoryCode { get; set; }
        [StringLength(500)]
        public string ProfileCategoryName { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
