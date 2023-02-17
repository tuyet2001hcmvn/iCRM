using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class QuestionSearchViewModel
    {
        public Guid Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionBankCode")]
        public int? QuestionBankCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Question")]
        public string Question { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Answer")]
        public string Answer { get; set; }
       
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerC")]
        public string AnswerC { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerB")]
        public string AnswerB { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        //public Guid QuestionCategoryId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public string QuestionCategoryId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public string QuestionCategoryName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string DepartmentId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string DepartmentName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }
    }
}
