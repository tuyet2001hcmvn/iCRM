using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ChangeDataLogModel", Schema = "utilities")]
    [Index(nameof(PrimaryKey), Name = "p1")]
    public partial class ChangeDataLogModel
    {
        [Key]
        public Guid LogId { get; set; }
        [StringLength(200)]
        public string TableName { get; set; }
        public Guid? PrimaryKey { get; set; }
        [StringLength(400)]
        public string FieldName { get; set; }
        [Column(TypeName = "ntext")]
        public string OldData { get; set; }
        [Column(TypeName = "ntext")]
        public string NewData { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
    }
}
