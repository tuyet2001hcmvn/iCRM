using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CategoryModel", Schema = "tSale")]
    public partial class CategoryModel
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        public int? ProductTypeId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        [StringLength(100)]
        public string ImageUrl { get; set; }
        public Guid? CompanyId { get; set; }
        public bool? IsTrackTrend { get; set; }
    }
}
