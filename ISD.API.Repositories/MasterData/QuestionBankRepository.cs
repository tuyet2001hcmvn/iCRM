using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.MasterData
{
    public class QuestionBankRepository : GenericRepository<QuestionBankModel>, IQuestionBankRepository
    {
        public QuestionBankRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<CatalogModel> GetDepartment()
        {
            return context.CatalogModels.Where(s => s.CatalogTypeCode == Constant.QuestionDepartmentTypeCode && s.Actived == true).OrderBy(s => s.OrderIndex);
        }

        public IQueryable<CatalogModel> GetQuestionCategoriesAvailable()
        {
            return context.CatalogModels.Where(s => s.CatalogTypeCode == Constant.QuestionCatalogTypeCode && s.Actived == true).OrderBy(s=>s.OrderIndex);
        }

        public IQueryable<QuestionBankModel> Search(int? questionBankCode, string question, Guid questionCategoryId, bool? actived, out int totalRow)
        {
            totalRow = 0;
            var questions = from q in context.QuestionBankModels.Include(t=>t.QuestionCategory).Include(t => t.CreateByNavigation).Include(t => t.LastEditByNavigation).Include(t => t.Department)
                            where (questionBankCode == null || q.QuestionBankCode == questionBankCode) &&
                            (question == null || question == "" || q.Question.Contains(question)) &&
                             (questionCategoryId == Guid.Empty || q.QuestionCategoryId==(questionCategoryId)) &&
                             (actived == null || q.Actived == actived)
                            orderby  (q.QuestionBankCode) descending
                            select q;
            totalRow = questions.Count();                 
            return questions;
        }
    }
}
