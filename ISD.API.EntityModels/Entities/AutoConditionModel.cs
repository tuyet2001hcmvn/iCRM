using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AutoConditionModel", Schema = "Task")]
    public partial class AutoConditionModel
    {
        [Key]
        public Guid AutoConditionId { get; set; }
        public Guid? StatusTransitionId { get; set; }
        [Column("AdditionalSQLText")]
        [StringLength(50)]
        public string AdditionalSqltext { get; set; }
        [StringLength(50)]
        public string ConditionType { get; set; }
        [StringLength(500)]
        public string Field { get; set; }
        [StringLength(50)]
        public string ComparisonType { get; set; }
        [StringLength(10)]
        public string ValueType { get; set; }
        [StringLength(500)]
        public string Value { get; set; }
        [Column("SQLText")]
        [StringLength(500)]
        public string Sqltext { get; set; }
        public int? OrderIndex { get; set; }
    }
}
