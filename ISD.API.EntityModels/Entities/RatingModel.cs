using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("RatingModel", Schema = "Task")]
    public partial class RatingModel
    {
        [Key]
        public Guid RatingId { get; set; }
        [StringLength(50)]
        public string RatingTypeCode { get; set; }
        public Guid? ReferenceId { get; set; }
        [StringLength(50)]
        public string Ratings { get; set; }
        public string Reviews { get; set; }
        [StringLength(500)]
        public string FullName { get; set; }
        [StringLength(500)]
        public string PhoneNumber { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
