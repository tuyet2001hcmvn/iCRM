using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public class TargetGroupRepository : GenericRepository<TargetGroupModel>, ITargetGroupRepository
    {
        public TargetGroupRepository(ICRMDbContext context) : base(context)
        {
        }

        public TargetGroupModel GetByCode(int code)
        {
            return dbSet.FirstOrDefault(t => t.TargetGroupCode == code);
        }

        public IEnumerable<TargetGroupModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, int page, int pageSize, string type)
        {
            //if (targetGroupCode == null && string.IsNullOrEmpty(targetGroupName) && actived == null)
            //{
            //    var targetGroupList1 = from t in context.TargetGroupModels
            //                           orderby (t.TargetGroupCode) descending
            //                           select t;
            //    var list1 = targetGroupList1.Skip(page * pageSize - pageSize).Take(pageSize).ToList();
            //    return list1;
            //}
            var targetGroupList = from t in context.TargetGroupModels.Include(s=>s.CreateByNavigation)
                                  where (targetGroupCode == null || t.TargetGroupCode == targetGroupCode) &&
                                   (string.IsNullOrEmpty(targetGroupName) || t.TargetGroupName.Contains(targetGroupName)) &&
                                    (actived == null || t.Actived == actived)
                                    && t.Type == type
                                  orderby (t.TargetGroupCode) descending
                                  select t;

            var list = targetGroupList.Skip(page * pageSize - pageSize).Take(pageSize).ToList();
            return list;
        }
        public int  GetTotalRow(int? targetGroupCode, string targetGroupName, bool? actived, string type)
        {
            if (targetGroupCode == null && string.IsNullOrEmpty(targetGroupName) && actived == null)
            {
                var targetGroupList1 = from t in context.TargetGroupModels
                                       where t.Type == type
                                       orderby (t.TargetGroupCode) descending
                                       select t;
                int totalRow1 = targetGroupList1.Count();
                return totalRow1;
            }
            var targetGroupList = from t in context.TargetGroupModels
                                  where (targetGroupCode == null || t.TargetGroupCode == targetGroupCode) &&
                                   (string.IsNullOrEmpty(targetGroupName) || t.TargetGroupName.Contains(targetGroupName)) &&
                                    (actived == null || t.Actived == actived)
                                  orderby (t.TargetGroupCode) descending
                                  select t;

            int totalRow = targetGroupList.Count();
            return totalRow;
        }

        public IQueryable<TargetGroupModel> GetAll(string Type)
        {
            return context.TargetGroupModels.Where(t => t.Actived == true && t.Type == Type);
        }
    }
}
