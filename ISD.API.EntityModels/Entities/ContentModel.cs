using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ContentModel", Schema = "Marketing")]
    public partial class ContentModel
    {
        [Key]
        public Guid Id { get; set; }
        public int ContentCode { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(500)]
        public string ContentName { get; set; }
        public Guid FromEmailAccountId { get; set; }
        public string SentFrom { get; set; }
        [Required]
        [StringLength(50)]
        public string SenderName { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [Required]
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        [StringLength(50)]
        public string CatalogCode { get; set; }
        [StringLength(50)]
        public string EmailType { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.ContentModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.ContentModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
    }
}
