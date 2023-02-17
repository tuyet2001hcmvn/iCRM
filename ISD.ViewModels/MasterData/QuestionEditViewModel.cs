using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class QuestionEditViewModel
    {
        public Guid Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionBankCode")]
        public int QuestionBankCode { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionContent")]
        public string Question { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Answer")]
        public string Answer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerC")]
        public string AnswerC { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerB")]
        public string AnswerB { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public Guid? QuestionCategoryId { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? DepartmentId { get; set; }
    }
}
