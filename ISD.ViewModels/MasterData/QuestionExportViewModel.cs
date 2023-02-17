using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class QuestionExportViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionBankCode")]
        public int QuestionBankCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionContent")]
        public string Question { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Answer")]
        public string Answer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerC")]
        public string AnswerC { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerB")]
        public string AnswerB { get; set; }
        [Display( Name = "Nhân viên")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public string QuestionCategoryName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string DepartmentName { get; set; }
    }

    public class QuestionResultViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionBankCode")]
        public int QuestionBankCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionContent")]
        public string Question { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Answer")]
        public string Answer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerC")]
        public string AnswerC { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AnswerB")]
        public string AnswerB { get; set; }
        [Display(Name = "Nhân viên")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuestionCategoryName")]
        public string QuestionCategoryName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string DepartmentName { get; set; }
    }
}
