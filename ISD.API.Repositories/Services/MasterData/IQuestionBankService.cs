using ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.MasterData
{
    public interface IQuestionBankService
    {
        QuestionViewModel Create(QuestionCreateViewModel obj);
        void Update(Guid id, QuestionEditViewModel obj);
        QuestionViewModel GetById(Guid id);
        List<QuestionCategoryViewModel> GetQuestionCategoriesAvailable();
        List<DepartmentViewModel> GetDepartment();
        List<QuestionViewModel> Search(int? questionBankCode, string question, Guid questionCategoryId, bool? actived, int pageIndex, int pageSize, out int totalRow);
    }
}
