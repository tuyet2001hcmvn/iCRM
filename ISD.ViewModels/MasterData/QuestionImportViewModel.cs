using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class QuestionImportViewModel
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
        public string QuestionCategoryCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
