using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public int QuestionBankCode { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerC { get; set; }
        public string AnswerB { get; set; }
        public string Actived { get; set; }
        public string QuestionCategoryId { get; set; }
        public string QuestionCategoryName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
