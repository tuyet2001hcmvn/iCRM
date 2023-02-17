using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.MasterData
{
    public interface IQuestionBankRepository
    {
        IQueryable<CatalogModel> GetQuestionCategoriesAvailable();
        IQueryable<CatalogModel> GetDepartment();
        IQueryable<QuestionBankModel> Search(int? questionBankCode, string question, Guid questionCategoryId, bool? actived, out int totalRow);
    }
}
