using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CatalogModel", Schema = "tMasterData")]
    public partial class CatalogModel
    {
        public CatalogModel()
        {
            CampaignModels = new HashSet<CampaignModel>();
            QuestionBankModelDepartments = new HashSet<QuestionBankModel>();
            QuestionBankModelQuestionCategories = new HashSet<QuestionBankModel>();
        }

        [Key]
        public Guid CatalogId { get; set; }
        [StringLength(100)]
        public string CatalogTypeCode { get; set; }
        public string CatalogCode { get; set; }
        [Column("CatalogText_en")]
        [StringLength(1000)]
        public string CatalogTextEn { get; set; }
        [Column("CatalogText_vi")]
        [StringLength(1000)]
        public string CatalogTextVi { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(CampaignModel.StatusNavigation))]
        public virtual ICollection<CampaignModel> CampaignModels { get; set; }
        [InverseProperty(nameof(QuestionBankModel.Department))]
        public virtual ICollection<QuestionBankModel> QuestionBankModelDepartments { get; set; }
        [InverseProperty(nameof(QuestionBankModel.QuestionCategory))]
        public virtual ICollection<QuestionBankModel> QuestionBankModelQuestionCategories { get; set; }
    }
}
