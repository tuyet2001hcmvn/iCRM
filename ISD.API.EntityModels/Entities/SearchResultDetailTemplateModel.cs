using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SearchResultDetailTemplateModel", Schema = "utilities")]
    public partial class SearchResultDetailTemplateModel
    {
        [Key]
        public Guid SearchResultDetailTemplateId { get; set; }
        public Guid? SearchResultTemplateId { get; set; }
        public int? PivotArea { get; set; }
        [StringLength(200)]
        public string FieldName { get; set; }
        [StringLength(200)]
        public string Caption { get; set; }
        [Column("CellFormat_FormatType")]
        [StringLength(200)]
        public string CellFormatFormatType { get; set; }
        [Column("CellFormat_FormatString")]
        [StringLength(200)]
        public string CellFormatFormatString { get; set; }
        public int? AreaIndex { get; set; }
        public bool? Visible { get; set; }
        public int? Width { get; set; }

        [ForeignKey(nameof(SearchResultTemplateId))]
        [InverseProperty(nameof(SearchResultTemplateModel.SearchResultDetailTemplateModels))]
        public virtual SearchResultTemplateModel SearchResultTemplate { get; set; }
    }
}
