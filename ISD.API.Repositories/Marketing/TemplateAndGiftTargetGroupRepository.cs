using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface ITemplateAndGiftTargetGroupRepository
    {
        IQueryable<TemplateAndGiftTargetGroupModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, out int totalRow);
    }
    public class TemplateAndGiftTargetGroupRepository : GenericRepository<TemplateAndGiftTargetGroupModel>, ITemplateAndGiftTargetGroupRepository
    {
        public TemplateAndGiftTargetGroupRepository(ICRMDbContext context) : base(context)
        {
        }
        public IQueryable<TemplateAndGiftTargetGroupModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, out int totalRow)
        {
            totalRow = 0;
            var list = from targetGroup in context.TemplateAndGiftTargetGroupModels.Include(s => s.CreateByNavigation).Include(s => s.LastEditByNavigation)
                       where (targetGroupCode == null  || targetGroup.TargetGroupCode == targetGroupCode)
                       && (targetGroupName == null || targetGroupName == "" || targetGroup.TargetGroupName.Contains(targetGroupName))
                       && (actived == null || targetGroup.Actived == actived)
                       orderby (targetGroup.TargetGroupCode) descending
                       select targetGroup;
            totalRow = list.Count();
            return list;
        }
    }
}
