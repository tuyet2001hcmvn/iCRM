using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class QuestionCreateViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionContent")]
        public string Question { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Answer")]
        public string Answer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerC")]
        public string AnswerC { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerB")]
        public string AnswerB { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public Guid? QuestionCategoryId { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? DepartmentId { get; set; }
    }
}
