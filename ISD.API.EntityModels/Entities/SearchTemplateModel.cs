using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SearchTemplateModel", Schema = "utilities")]
    public partial class SearchTemplateModel
    {
        [Key]
        public Guid SearchTemplateId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PageId { get; set; }
        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; }
        public string SearchContent { get; set; }
    }
}
