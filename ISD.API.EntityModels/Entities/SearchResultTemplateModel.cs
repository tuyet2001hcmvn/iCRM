using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SearchResultTemplateModel", Schema = "utilities")]
    public partial class SearchResultTemplateModel
    {
        public SearchResultTemplateModel()
        {
            SearchResultDetailTemplateModels = new HashSet<SearchResultDetailTemplateModel>();
        }

        [Key]
        public Guid SearchResultTemplateId { get; set; }
        [Column("isSystem")]
        public bool? IsSystem { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? PageId { get; set; }
        [StringLength(100)]
        public string TemplateName { get; set; }
        public bool? IsDefaultTemplate { get; set; }
        [Column(TypeName = "ntext")]
        public string LayoutConfigs { get; set; }
        [StringLength(50)]
        public string OrderBy { get; set; }
        [StringLength(50)]
        public string TypeSort { get; set; }

        [InverseProperty(nameof(SearchResultDetailTemplateModel.SearchResultTemplate))]
        public virtual ICollection<SearchResultDetailTemplateModel> SearchResultDetailTemplateModels { get; set; }
    }
}
