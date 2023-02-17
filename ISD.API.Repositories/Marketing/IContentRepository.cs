using ISD.API.EntityModels.Entities;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public interface IContentRepository
    {
        IQueryable<ContentModel> Search(int? contentCode, string contentName, bool? actived,string Type);
        IQueryable<ContentModel> GetAll(string Type);
    }
}
