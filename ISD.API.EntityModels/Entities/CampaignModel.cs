using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CampaignModel", Schema = "Marketing")]
    public partial class CampaignModel
    {
        [Key]
        public Guid Id { get; set; }
        public int CampaignCode { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(255)]
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public Guid ContentId { get; set; }
        public Guid TargetGroupId { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        public Guid? Status { get; set; }
        [Column("isImmediately")]
        public bool? IsImmediately { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ScheduledToStart { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(Status))]
        [InverseProperty(nameof(CatalogModel.CampaignModels))]
        public virtual CatalogModel StatusNavigation { get; set; }
        public virtual ContentModel Content { get; set; }
        public virtual TargetGroupModel TargetGroup { get; set; }
    }
}
