using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("NewsCategoryModel", Schema = "tMasterData")]
    public partial class NewsCategoryModel
    {
        [Key]
        public Guid NewsCategoryId { get; set; }
        public int NewsCategoryCode { get; set; }
        [StringLength(200)]
        public string NewsCategoryName { get; set; }
        public string Description { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        [StringLength(4000)]
        public string ImageUrl { get; set; }
    }
}
