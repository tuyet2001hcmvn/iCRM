using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public class ContentRepository : GenericRepository<ContentModel>, IContentRepository
    {
        public ContentRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<ContentModel> GetAll(string Type)
        {
            return context.ContentModels.Where(c => c.Actived == true && c.Type == Type);
        }

        public IQueryable<ContentModel> Search(int? contentCode, string contentName, bool? actived, string Type)
        {
            var contents = from t in context.ContentModels.Include(s=>s.CreateByNavigation)
                           where (contentCode == null || t.ContentCode == contentCode) &&
                            (contentName==null || contentName=="" || t.ContentName.Contains(contentName)) &&
                             (actived == null || t.Actived == actived)
                             && t.Type == Type
                           orderby (t.ContentCode) descending
                           select t;

            return contents;
        }

    }
}
