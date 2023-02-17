using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels
{
    public class QuestionEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerC { get; set; }
        public string AnswerB { get; set; }
        [Required]
        public Guid? QuestionCategoryId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? LastEditBy { get; set; }
    }
}
