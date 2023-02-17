using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.UtilitiesViewModel.SearchTemplateViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Utilities
{
    public interface ISearchTemplateRepository
    {
        IQueryable<SearchTemplateModel> GetByAccount(Guid AccountId, Guid PageId);
    }
    public class SearchTemplateRepository : GenericRepository<SearchTemplateModel>, ISearchTemplateRepository
    {
        public SearchTemplateRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<SearchTemplateModel> GetByAccount(Guid AccountId, Guid PageId)
        {
            return context.SearchTemplateModels.Where(p => p.AccountId == AccountId && p.PageId == PageId).OrderBy(p => p.TemplateName);
        }

    }
}

