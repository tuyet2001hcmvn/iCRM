using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("QuestionBankModel", Schema = "tMasterData")]
    public partial class QuestionBankModel
    {
        [Key]
        public Guid Id { get; set; }
        public int QuestionBankCode { get; set; }
        [Required]
        [StringLength(2000)]
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerC { get; set; }
        public string AnswerB { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }
        public Guid? QuestionCategoryId { get; set; }
        public Guid? DepartmentId { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.QuestionBankModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        [InverseProperty(nameof(CatalogModel.QuestionBankModelDepartments))]
        public virtual CatalogModel Department { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.QuestionBankModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [ForeignKey(nameof(QuestionCategoryId))]
        [InverseProperty(nameof(CatalogModel.QuestionBankModelQuestionCategories))]
        public virtual CatalogModel QuestionCategory { get; set; }
    }
}
