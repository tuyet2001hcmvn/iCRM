using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("NewsModel", Schema = "tMasterData")]
    public partial class NewsModel
    {
        [Key]
        public Guid NewsId { get; set; }
        public int NewsCode { get; set; }
        public Guid? NewsCategoryId { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ScheduleTime { get; set; }
        [StringLength(4000)]
        public string ImageUrl { get; set; }
        [Column("isShowOnMobile")]
        public bool? IsShowOnMobile { get; set; }
        [Column("isShowOnWeb")]
        public bool? IsShowOnWeb { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        [StringLength(50)]
        public string Detail { get; set; }
        [StringLength(50)]
        public string Summary { get; set; }
    }
}
